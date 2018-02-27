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

        protected override bool HasLogs => false;

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => null;
        protected override string SilentArgs => "/S";
    }
}
