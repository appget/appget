using System.Threading.Tasks;
using AppGet.Installers;

namespace AppGet.Commands.Uninstall
{
    public class UninstallCommandHandler : ICommandHandler
    {
        private readonly IInstallService _installService;


        public UninstallCommandHandler(IInstallService installService)
        {
            _installService = installService;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            await _installService.Uninstall((UninstallOptions)commandOptions);
        }
    }
}