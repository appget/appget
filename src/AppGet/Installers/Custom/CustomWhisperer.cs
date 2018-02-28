using AppGet.Commands.Install;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Custom
{
    public class CustomWhisperer : InstallerWhispererBase
    {
        public CustomWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {

        }

        protected override string GetInstallArguments(InstallOptions installOptions, PackageManifest manifest)
        {
            // Passive is default
            var args = manifest.Args.Passive;

            if (installOptions.Silent)
            {
                args = manifest.Args.Silent;
            }
            else if (installOptions.Interactive)
            {
                args = manifest.Args.Interactive;
            }

            return args?.Trim();
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Custom;
        protected override bool HasLogs => false;

        protected override string InteractiveArgs => null;
        protected override string PassiveArgs => null;
        protected override string SilentArgs => null;
    }
}
