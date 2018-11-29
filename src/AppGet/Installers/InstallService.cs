using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Commands.Uninstall;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.Infrastructure.Eventing;
using AppGet.Installers.Events;
using AppGet.Installers.InstallerWhisperer;
using AppGet.Installers.UninstallerWhisperer;
using AppGet.Manifest;
using AppGet.Manifests;
using AppGet.Update;
using AppGet.Windows;
using AppGet.Windows.WindowsInstaller;
using NLog;

namespace AppGet.Installers
{
    public interface IInstallService
    {
        Task Install(PackageManifest packageManifest, InstallInteractivityLevel interactivityLevel);
        Task Uninstall(UninstallOptions uninstallOptions);
    }

    public class InstallService : IInstallService
    {
        private readonly Logger _logger;
        private readonly IFindInstaller _findInstaller;
        private readonly IPathResolver _pathResolver;
        private readonly IProcessController _processController;
        private readonly InstallerContextFactory _installerContextFactory;
        private readonly IFileTransferService _fileTransferService;
        private readonly WindowsInstallerClient _windowsInstallerClient;
        private readonly Func<InstallerBase[]> _installWhisperers;
        private readonly Func<UninstallerBase[]> _uninstallers;
        private readonly IHub _hub;
        private readonly IUnlocker _unlocker;
        private readonly NovoClient _novoClient;

        public InstallService(Logger logger, IFindInstaller findInstaller, IPathResolver pathResolver, IProcessController processController,
            InstallerContextFactory installerContextFactory,
            IFileTransferService fileTransferService, WindowsInstallerClient windowsInstallerClient, Func<InstallerBase[]> installWhisperers,
            Func<UninstallerBase[]> uninstallers, IHub hub, IUnlocker unlocker, NovoClient novoClient)
        {
            _logger = logger;
            _findInstaller = findInstaller;
            _pathResolver = pathResolver;
            _processController = processController;
            _installerContextFactory = installerContextFactory;
            _fileTransferService = fileTransferService;
            _windowsInstallerClient = windowsInstallerClient;
            _installWhisperers = installWhisperers;
            _uninstallers = uninstallers;
            _hub = hub;
            _unlocker = unlocker;
            _novoClient = novoClient;
        }

        public async Task Install(PackageManifest packageManifest, InstallInteractivityLevel interactivityLevel)
        {
            using (var context = _installerContextFactory.Build(packageManifest, interactivityLevel, PackageOperation.Install))
            {
                _logger.Info("Beginning installation of '{0}'", packageManifest);
                _hub.Publish(new InitializationInstallationEvent(packageManifest));

                var installer = _findInstaller.GetBestInstaller(packageManifest.Installers);
                var installerPath = await _fileTransferService.TransferFile(installer.Location, _pathResolver.TempFolder, installer.Sha256);

                var updates = await GetUpdate(packageManifest.Id, context.InstallerRecords);

                foreach (var update in updates)
                {
                    if (update?.InstallationPath != null)
                    {
                        _unlocker.UnlockFolder(update.InstallationPath, packageManifest.InstallMethod);
                    }
                }

                var availableUpdates = updates.Where(c => c.Status == UpdateStatus.Available);

                if (availableUpdates.Any())
                {
                    _logger.Info("Updating {0} to {1}. Currently Installed: {2}", packageManifest.Name, packageManifest.Version,
                        updates.First().InstalledVersion);
                }

                var whisperer = _installWhisperers().First(c => c.InstallMethod == packageManifest.InstallMethod);
                whisperer.Initialize(packageManifest, installerPath);

                try
                {
                    context.Process = RunInstaller(interactivityLevel, packageManifest, whisperer);
                }
                catch (InstallerException ex)
                {
                    context.Exception = ex;
                    throw;
                }

                _logger.Info("Installation completed successfully for '{0}'", packageManifest);
                _hub.Publish(new InstallationSuccessfulEvent(packageManifest));
            }
        }

        public async Task Uninstall(UninstallOptions uninstallOptions)
        {
            using (var context = _installerContextFactory.Build(uninstallOptions.PackageId, uninstallOptions.InteractivityLevel, PackageOperation.Uninstall))
            {
                _logger.Info("Beginning uninstallation of " + uninstallOptions.PackageId);

                var uninstallRecords = await _novoClient.GetUninstall(context.InstallerRecords, uninstallOptions.PackageId);

                if (!uninstallRecords.Any())
                {
                    _logger.Warn("Couldn't find an installed package matching '{0}'", uninstallOptions.PackageId);
                    return;
                }

                if (uninstallRecords.Count != 1)
                {
                    _logger.Warn("Found more than one installed package for {0}", uninstallOptions.PackageId);

                    foreach (var record in uninstallRecords)
                    {
                        _logger.Warn("{0} {1}", record.DisplayName, record.DisplayVersion);
                    }

                    return;
                }

                var uninstallRecord = uninstallRecords.Single();

                if (uninstallRecord.InstallationPath != null)
                {
                    _unlocker.UnlockFolder(uninstallRecord.InstallationPath, uninstallRecord.InstallMethod);
                }

                var keys = _windowsInstallerClient.GetKey(uninstallRecord.WindowsInstallerId);

                var whisperer = _uninstallers().First(c => c.InstallMethod == uninstallRecord.InstallMethod);
                whisperer.InitUninstaller(keys, uninstallRecord);

                RunInstaller(uninstallOptions.InteractivityLevel, new PackageManifest(), whisperer);
            }
        }

        private async Task<List<PackageUpdate>> GetUpdate(string packageId, IEnumerable<WindowsInstallerRecord> records)
        {
            _logger.Debug("Getting list of installed application");
            var result = await _novoClient.GetUpdate(records, packageId);
            return result.ToList();
        }


        private Process RunInstaller(InstallInteractivityLevel interactivity, PackageManifest packageManifest, IInstaller whisperer)
        {
            _hub.Publish(new ExecutingInstallerEvent(packageManifest));

            var logPath = _pathResolver.GetInstallerLogFile(packageManifest.Id);

            string GetLoggingArgs()
            {
                var template = packageManifest.Args?.Log ?? whisperer.LogArgs;
                return template?.Replace("{path}", $"\"{logPath}\"");
            }

            var loggingArgs = GetLoggingArgs();

            var installArgs = GetInstallerArguments(interactivity, packageManifest, whisperer);

            if (loggingArgs != null)
            {
                _logger.Info($"Writing installer log files to {logPath}");
                installArgs = $"{installArgs.Trim()} {loggingArgs.Trim()}";
            }


            var process = _processController.Start(whisperer.GetProcessPath(), installArgs);
            _logger.Info("Waiting for installation to complete ...");
            _processController.WaitForExit(process);

            if (process.ExitCode != 0)
            {
                var logFile = installArgs == null ? null : logPath;
                whisperer.ExitCodes.TryGetValue(process.ExitCode, out var exitReason);

                throw new InstallerException(process.ExitCode, packageManifest, exitReason, logFile);
            }

            return process;
        }

        private string GetInstallerArguments(InstallInteractivityLevel interactivity, PackageManifest manifest, IInstaller installer)
        {
            var effectiveInteractivity = GetInteractivelyLevel(interactivity, manifest, installer);

            switch (effectiveInteractivity)
            {
                case InstallInteractivityLevel.Silent:
                    return $"{installer.SilentArgs} {manifest.Args?.Silent}";
                case InstallInteractivityLevel.Interactive:
                    return $"{installer.InteractiveArgs} {manifest.Args?.Interactive}";
                case InstallInteractivityLevel.Passive:
                    return $"{installer.PassiveArgs} {manifest.Args?.Passive}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectiveInteractivity));
            }
        }

        private InstallInteractivityLevel GetInteractivelyLevel(InstallInteractivityLevel interactivity, PackageManifest manifest, IInstaller installer)
        {
            bool SupportsSilent()
            {
                return manifest.Args?.Silent != null || installer.SilentArgs != null;
            }

            bool SupportsPassive()
            {
                return manifest.Args?.Passive != null || installer.PassiveArgs != null;
            }

            if (interactivity == InstallInteractivityLevel.Silent && !SupportsSilent())
            {
                if (SupportsPassive())
                {
                    _logger.Info("Silent install is not supported by installer. Switching to Passive");
                    return InstallInteractivityLevel.Passive;
                }

                _logger.Warn("Silent or Passive install is not supported by installer. Switching to Interactive");
                return InstallInteractivityLevel.Interactive;
            }

            if (interactivity == InstallInteractivityLevel.Passive && !SupportsPassive())
            {
                if (SupportsSilent())
                {
                    _logger.Info("Passive install is not supported by installer. Switching to Silent.");
                    return InstallInteractivityLevel.Silent;
                }

                _logger.Warn("Silent or Passive install is not supported by installer. Switching to Interactive");
                return InstallInteractivityLevel.Interactive;
            }

            return interactivity;
        }
    }
}