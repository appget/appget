using System;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Nsis
{
    public class NsisWhisperer : InstallerWhispererBase
    {
        public NsisWhisperer(IProcessController processController, Logger logger)
            : base(processController, logger)
        {

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
