using System.ComponentModel;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.Exceptions;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers
{
//    public class ArgumentBuilder
//    {
//        private readonly char _anchorChar;
//        private readonly char _valueAssignment;
//
//        private string result = "";
//
//        public ArgumentBuilder(char anchorChar = '/', char valueAssignment = '=')
//        {
//            _anchorChar = anchorChar;
//            _valueAssignment = valueAssignment;
//        }
//
//        public void AddFlag(string value)
//        {
//            result = result.Trim();
//            result += $" {_anchorChar}{value.Trim()} ";
//        }
//
//        public void AddValue(string key, string value)
//        {
//            result = result.Trim();
//            result += $" {_anchorChar}{key.Trim()}{_valueAssignment}{value.Trim()} ";
//        }
//
//        public string Build()
//        {
//            return result.Trim();
//        }
//
//    }

    public abstract class InstallerWhispererBase : IInstallerWhisperer
    {
        private readonly IProcessController _processController;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        protected InstallerWhispererBase(IProcessController processController, IPathResolver pathResolver, Logger logger)
        {
            _processController = processController;
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public virtual void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions)
        {
            //Command line args: http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline

            var logFile = _pathResolver.GetInstallerLogFile(packageManifest);

            _logger.Debug($"Writing installer log files to {logFile}");
            Execute(installerLocation, GetInstallArguments(installOptions));
        }


        public abstract void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions);
        public abstract bool CanHandle(InstallMethodType installMethod);


        private string GetInstallArguments(InstallOptions installOptions)
        {
            var args = "";
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
                args = UnattendedArgs;
            }

            var loggingArgs = LoggingArgs;

            if (!string.IsNullOrWhiteSpace(loggingArgs))
            {
                args = $"{args.Trim()} {loggingArgs.Trim()}";
            }

            return args.Trim();
        }

        protected void Execute(string executable, string args)
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

        protected abstract string InteractiveArgs { get; }
        protected abstract string UnattendedArgs { get; }
        protected abstract string SilentArgs { get; }
        protected abstract string LoggingArgs { get; }

        private void OnOutputDataReceived(string message)
        {
            _logger.Info(message);
        }

        private void OnErrorDataReceived(string message)
        {
            _logger.Error(message);
        }
    }
}
