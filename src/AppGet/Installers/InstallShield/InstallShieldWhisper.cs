using AppGet.HostSystem;
using AppGet.Installers.Msi;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.InstallShield
{
    public class InstallShieldWhisperer : MsiWhisperer
    {
        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallShield;

        public InstallShieldWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }
    }
}