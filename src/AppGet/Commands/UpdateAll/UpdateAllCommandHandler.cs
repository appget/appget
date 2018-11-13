using System.Threading.Tasks;
using AppGet.Infrastructure.Composition;
using AppGet.Update;

namespace AppGet.Commands.UpdateAll
{
    [Handles(typeof(UpdateAllOptions))]
    public class UpdateAllCommandHandler : ICommandHandler
    {
        private readonly UpdateService _updateService;

        public UpdateAllCommandHandler(UpdateService updateService)
        {
            _updateService = updateService;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var updateOptions = (UpdateAllOptions)commandOptions;
            await _updateService.UpdateAllPackages(updateOptions.GetInteractivityLevel());
        }
    }
}