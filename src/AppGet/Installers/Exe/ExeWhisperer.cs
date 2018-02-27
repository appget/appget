using System;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Exe
{
    public class ExeWhisperer : InstallerWhispererBase
    {
        public ExeWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {

        }

        protected override string GetInstallArguments(InstallOptions installOptions, PackageManifest manifest)
        {
            // Passive is default
            string args = manifest.Args.Passive;

            if (installOptions.Silent)
            {
                args = manifest.Args.Silent;
            }
            else if (installOptions.Interactive)
            {
                args = "";
            }

            return args?.Trim();
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Custom;
        protected override bool HasLogs => false;

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        protected override string InteractiveArgs => null;
        protected override string PassiveArgs => null;
        protected override string SilentArgs => null;
    }
}
