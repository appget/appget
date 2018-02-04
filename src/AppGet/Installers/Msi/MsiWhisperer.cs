using System;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Msi
{
    public class MsiWhisperer : InstallerWhispererBase
    {
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public MsiWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, logger)
        {
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public override void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions)
        {
            var logFile = _pathResolver.GetInstallerLogFile(packageManifest);
            var args = GetArgs(installerLocation, logFile);

            _logger.Debug("Writing MSI log files to {0}", logFile);

            Execute("msiexec", args);
        }

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException("MSI Uninstall is not currently supported.");
        }

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.MSI;
        }

        private static string GetArgs(string msiPath, string logFile)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/aa367988(v=vs.85).aspx
            return $"/i \"{msiPath}\" /qb /norestart /Liwemoar! \"{logFile}\"";
        }
    }
}
