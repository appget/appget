using System.Threading.Tasks;
using AppGet.Infrastructure.Composition;
using AppGet.Update;

namespace AppGet.Commands.Update
{
    [Handles(typeof(UpdateOptions))]
    public class UpdateCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;

        public UpdateCommandHandler(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var updateOptions = (UpdateOptions)commandOptions;
            await _updateService.UpdatePackage(updateOptions.PackageId, updateOptions.Tag, updateOptions.Repository, updateOptions.GetInteractivityLevel());
        }
    }
}