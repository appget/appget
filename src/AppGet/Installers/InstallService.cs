using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Commands.Uninstall;
using AppGet.FileTransfer;
using AppGet.Infrastructure.Eventing;
using AppGet.Installers.Events;
using AppGet.Installers.InstallerWhisperer;
using AppGet.Installers.UninstallerWhisperer;
using AppGet.Manifest;
using AppGet.Manifests;
using AppGet.Update;
using AppGet.Windows;
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
        private readonly IProcessController _processController;
        private readonly InstallerContextFactory _installerContextFactory;
        private readonly IFileTransferService _fileTransferService;
        private readonly Func<InstallerBase[]> _installWhisperers;
        private readonly Func<UninstallerBase[]> _uninstallers;
        private readonly IHub _hub;
        private readonly IUnlocker _unlocker;
        private readonly NovoClient _novoClient;
        private readonly InstallerArgFactory _argFactory;

        public InstallService(Logger logger, IFindInstaller findInstaller, IProcessController processController,
            InstallerContextFactory installerContextFactory,
            IFileTransferService fileTransferService, Func<InstallerBase[]> installWhisperers,
            Func<UninstallerBase[]> uninstallers, IHub hub, IUnlocker unlocker, NovoClient novoClient, InstallerArgFactory argFactory)
        {
            _logger = logger;
            _findInstaller = findInstaller;
            _processController = processController;
            _installerContextFactory = installerContextFactory;
            _fileTransferService = fileTransferService;
            _installWhisperers = installWhisperers;
            _uninstallers = uninstallers;
            _hub = hub;
            _unlocker = unlocker;
            _novoClient = novoClient;
            _argFactory = argFactory;
        }

        public async Task Install(PackageManifest packageManifest, InstallInteractivityLevel interactivityLevel)
        {
            using (var context = _installerContextFactory.Build(packageManifest, interactivityLevel, PackageOperation.Install))
            {
                _logger.Info("Beginning installation of '{0}'", packageManifest);
                _hub.Publish(new InstallationInitializedEvent(packageManifest));

                var installer = _findInstaller.GetBestInstaller(packageManifest.Installers);

                var installerPath = await _fileTransferService.TransferFile(installer.Location, installer.Sha256);

                _logger.Debug("Getting list of installed application");
                var updates = await _novoClient.GetUpdate(context.InstallerRecords, packageManifest.Id);

                var availableUpdates = updates.Where(c => c.Status == UpdateStatus.Available);

                if (availableUpdates.Any())
                {
                    _logger.Info("Updating {0} to {1}. Currently Installed: {2}", packageManifest.Name, packageManifest.Version, updates.First().InstalledVersion);
                }

                var whisperer = _installWhisperers().First(c => c.InstallMethod == packageManifest.InstallMethod);

                context.Whisperer = whisperer;
                whisperer.Initialize(packageManifest, installerPath);


                foreach (var update in updates)
                {
                    _unlocker.UnlockFolder(update.InstallationPath, packageManifest.InstallMethod);
                }

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

                var whisperer = _uninstallers().First(c => c.InstallMethod == uninstallRecord.InstallMethod);
                context.Whisperer = whisperer;
                whisperer.InitUninstaller(uninstallRecord);

                _unlocker.UnlockFolder(uninstallRecord.InstallationPath, uninstallRecord.InstallMethod);

                try
                {
                    context.Process = RunInstaller(uninstallOptions.InteractivityLevel, new PackageManifest { Id = uninstallOptions.PackageId }, whisperer);
                }
                catch (InstallerException ex)
                {
                    context.Exception = ex;
                    throw;
                }
            }
        }

        private Process RunInstaller(InstallInteractivityLevel interactivity, PackageManifest packageManifest, IInstaller whisperer)
        {
            _hub.Publish(new ExecutingInstallerEvent(packageManifest));

            var installArgs = _argFactory.GetInstallerArguments(interactivity, packageManifest, whisperer);

            var process = _processController.Start(whisperer.GetProcessPath(), installArgs.Arguments);
            _logger.Info("Waiting for installation to complete ...");
            _processController.WaitForExit(process);

            if (process.ExitCode != 0)
            {
                whisperer.ExitCodes.TryGetValue(process.ExitCode, out var exitReason);
                throw new InstallerException(process.ExitCode, packageManifest, exitReason, installArgs.LogFile);
            }

            return process;
        }
    }
}