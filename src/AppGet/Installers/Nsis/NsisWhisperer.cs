using System;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Nsis
{
    public class NsisWhisperer : InstallerWhispererBase
    {
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public NsisWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, logger)
        {
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public override void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions)
        {
            Execute(installerLocation, "/S");
        }

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.NSIS;
        }
    }
}
