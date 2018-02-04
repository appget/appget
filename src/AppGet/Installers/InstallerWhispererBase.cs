using System.ComponentModel;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.Exceptions;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers
{
    public abstract class InstallerWhispererBase : IInstallerWhisperer
    {
        private readonly IProcessController _processController;
        private readonly Logger _logger;

        protected InstallerWhispererBase(IProcessController processController, Logger logger)
        {
            _processController = processController;
            _logger = logger;
        }

        public abstract void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions);
        public abstract void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions);
        public abstract bool CanHandle(InstallMethodType installMethod);

        protected virtual void Execute(string executable, string args)
        {
            try
            {
                var process = _processController.Start(executable, args, OnOutputDataReceived, OnErrorDataReceived);
                _processController.WaitForExit(process);

                if (process.ExitCode != 0)
                {
                    throw new AppGetException($"Installer '{process.ProcessName}' returned with a non-zero exit code. code: {process.ExitCode}");
                }
            }
            catch (Win32Exception e)
            {
                _logger.Error($"{e.Message}, try running AppGet as an administartor");
                throw;
            }
        }

        protected virtual void OnOutputDataReceived(string message)
        {
            _logger.Info(message);
        }

        protected virtual void OnErrorDataReceived(string message)
        {
            _logger.Error(message);
        }
    }
}
