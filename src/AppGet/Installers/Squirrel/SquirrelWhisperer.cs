using System;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Squirrel
{
    public class SquirrelWhisperer : InstallerWhispererBase
    {
        public SquirrelWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {

        }

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(InstallMethodTypes installMethod)
        {
            return installMethod == InstallMethodTypes.Squirrel;
        }

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => null;
        protected override string SilentArgs => "/S";
    }
}
