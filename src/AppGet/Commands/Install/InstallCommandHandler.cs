using System.Threading.Tasks;
using AppGet.Infrastructure.Composition;
using AppGet.Installers;
using AppGet.PackageRepository;

namespace AppGet.Commands.Install
{
    [Handles(typeof(InstallOptions))]
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IInstallService _installService;

        public InstallCommandHandler(IPackageRepository packageRepository, IInstallService installService)
        {
            _packageRepository = packageRepository;
            _installService = installService;
        }


        public async Task Execute(AppGetOption commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            var package = await _packageRepository.GetAsync(installOptions.PackageId, installOptions.Tag, installOptions.Repository);
            await _installService.Install(package, installOptions.GetInteractivityLevel());
        }
    }
}