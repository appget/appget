using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers
{
    public abstract class InstallerWhispererBase : IInstallerWhisperer
    {
        private readonly IProcessController _processController;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public virtual Dictionary<int, ExistReason> ExitCodes => new Dictionary<int, ExistReason>();

        protected abstract InstallMethodTypes InstallMethod { get; }


        protected abstract bool HasLogs { get; }


        protected InstallerWhispererBase(IProcessController processController, IPathResolver pathResolver, Logger logger)
        {
            _processController = processController;
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public virtual void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions)
        {
            var process = StartProcess(installerLocation, GetInstallArguments(installOptions, packageManifest));
            _logger.Info("Waiting for installation to complete ...");
            _processController.WaitForExit(process);
            var logFile = HasLogs ? _pathResolver.GetInstallerLogFile(packageManifest.Id) : null;

            if (process.ExitCode != 0)
            {
                ExitCodes.TryGetValue(process.ExitCode, out var exitReason);
                throw new InstallerException(process, packageManifest, exitReason, logFile);
            }
        }

        protected virtual Process StartProcess(string installerLocation, string args)
        {
            return _processController.Start(installerLocation, args);
        }


        public virtual void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {

        }

        public bool CanHandle(InstallMethodTypes installMethod)
        {
            return installMethod == InstallMethod;
        }

        private string GetInstallArguments(InstallOptions installOptions, PackageManifest manifest)
        {
            if (installOptions.Silent && !SupportsSilent(manifest))
            {
                if (SupportsPassive(manifest))
                {
                    installOptions.Passive = true;
                    _logger.Warn("Silent install is not supported by installer. Falling back to Passive");
                }
                else
                {
                    installOptions.Interactive = true;
                    _logger.Warn("Silent or Passive install is not supported by installer. Falling back to Interactive");
                }
            }

            if (installOptions.Passive && !SupportsPassive(manifest))
            {
                if (SupportsSilent(manifest))
                {
                    installOptions.Silent = true;
                    _logger.Warn("Passive install is not supported by installer. Falling back to Silent.");
                }
                else
                {
                    installOptions.Interactive = true;
                    _logger.Warn("Silent or Passive install is not supported by installer. Falling back to Interactive");
                }
            }


            string args;
            if (installOptions.Silent)
            {
                args = $"{SilentArgs} {manifest.Args?.Silent}";
            }
            else if (installOptions.Interactive)
            {
                args = $"{InteractiveArgs} {manifest.Args?.Interactive}";
            }
            else
            {
                args = $"{PassiveArgs} {manifest.Args?.Passive}";
            }

            if (HasLogs)
            {
                var logFile = _pathResolver.GetInstallerLogFile(installOptions.PackageId);
                var loggingArgs = GetLoggingArgs(logFile);
                _logger.Debug($"Writing installer log files to {logFile}");
                args = $"{args.Trim()} {loggingArgs.Trim()}";
            }

            return args.Trim();
        }

        private bool SupportsSilent(PackageManifest manifest)
        {
            return manifest.Args?.Silent != null || SilentArgs != null;
        }

        private bool SupportsPassive(PackageManifest manifest)
        {
            return manifest.Args?.Passive != null || PassiveArgs != null;
        }

        protected abstract string InteractiveArgs { get; }
        protected abstract string PassiveArgs { get; }
        protected abstract string SilentArgs { get; }

        protected virtual string GetLoggingArgs(string path)
        {
            return null;
        }
    }
}
