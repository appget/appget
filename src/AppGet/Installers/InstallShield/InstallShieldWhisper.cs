using AppGet.HostSystem;
using AppGet.Installers.Msi;
using AppGet.Manifest;
using AppGet.Windows;
using NLog;

namespace AppGet.Installers.InstallShield
{
    public class InstallShieldWhisperer : MsiWhisperer
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallShield;

        public InstallShieldWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }
    }
}