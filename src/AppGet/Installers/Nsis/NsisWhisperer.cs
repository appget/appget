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

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "/S";
        protected override string SilentArgs => "/S";
    }
}
