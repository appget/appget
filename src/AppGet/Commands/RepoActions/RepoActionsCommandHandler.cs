using System.Threading.Tasks;
using AppGet.Infrastructure.Composition;
using AppGet.PackageRepository;

namespace AppGet.Commands.RepoActions
{
    [Handles(typeof(RepoActionsOptions))]
    public class RepoActionsCommandHandler : ICommandHandler
    {
        private readonly PrivateRepositoryService _privateRepositoryService;

        public RepoActionsCommandHandler(PrivateRepositoryService privateRepositoryService)
        {
            _privateRepositoryService = privateRepositoryService;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var addRepoOptions = (RepoActionsOptions)commandOptions;
            await _privateRepositoryService.AddRepository(addRepoOptions.ConnectionString);
        }
    }
}