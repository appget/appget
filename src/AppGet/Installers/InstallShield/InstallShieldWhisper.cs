using AppGet.HostSystem;
using AppGet.Installers.Msi;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.InstallShield
{
    public class InstallShieldWhisperer : MsiWhisperer
    {
        public InstallShieldWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }
    }
}
