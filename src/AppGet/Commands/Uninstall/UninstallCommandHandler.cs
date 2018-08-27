using System.Threading.Tasks;
using AppGet.Commands.CreateManifest;
using AppGet.Infrastructure.Composition;
using AppGet.Installers;

namespace AppGet.Commands.Uninstall
{
    [Handles(typeof(UninstallOptions))]
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