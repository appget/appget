using System.Collections.Generic;
using System.Diagnostics;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Installers.Inno;
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
            var logFile = _pathResolver.GetInstallerLogFile(packageManifest.Id);

            if (process.ExitCode != 0)
            {
                throw new InstallerException(process, packageManifest, logFile);
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


        protected virtual string GetInstallArguments(InstallOptions installOptions, PackageManifest manifest)
        {
            string args;
            if (installOptions.Silent)
            {
                args = SilentArgs;
            }
            else if (installOptions.Interactive)
            {
                args = InteractiveArgs;
            }
            else
            {
                args = PassiveArgs;
            }


            if (HasLogs)
            {
                var logFile = _pathResolver.GetInstallerLogFile(installOptions.PackageId);
                var loggingArgs = GetLoggingArgs(logFile);
                _logger.Debug($"Writing installer log files to {logFile}");
                args = $"{args.Trim()} {loggingArgs.Trim()}";
            }

            return args?.Trim();
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
