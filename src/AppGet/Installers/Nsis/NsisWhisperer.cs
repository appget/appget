using System;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Nsis
{
    public class NsisWhisperer : InstallerWhispererBase
    {
        public NsisWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {

        }

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.NSIS;
        }

        protected override string InteractiveArgs => "";
        protected override string UnattendedArgs => null;
        protected override string SilentArgs => "/S";
        protected override string LoggingArgs => null;
    }
}
