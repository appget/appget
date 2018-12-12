using System;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Infrastructure.Composition;
using AppGet.PackageRepository;

namespace AppGet.Commands.RepoActions
{
    [Handles(typeof(RepoActionsOptions))]
    public class RepoActionsCommandHandler : ICommandHandler
    {
        private readonly PrivateRepositoryService _privateRepositoryService;
        private readonly RepositoryRegistry _repositoryRegistry;

        public RepoActionsCommandHandler(PrivateRepositoryService privateRepositoryService, RepositoryRegistry repositoryRegistry)
        {
            _privateRepositoryService = privateRepositoryService;
            _repositoryRegistry = repositoryRegistry;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var repoActionsOptions = (RepoActionsOptions)commandOptions;

            switch (repoActionsOptions.Action.ToLowerInvariant().Trim())
            {
                case "add":
                    {
                        await _privateRepositoryService.AddRepository(repoActionsOptions.ConnectionString);
                        break;
                    }
                case "remove":
                    {
                        if (repoActionsOptions.Name == null && repoActionsOptions.Id == null)
                        {
                            throw new InvalidCommandParamaterException("Repository Name or ID is required.", repoActionsOptions);
                        }
                        await _privateRepositoryService.RemoveRepository(repoActionsOptions.Name, repoActionsOptions.Id);
                        break;
                    }
                case "list":
                    {
                        var repos = _repositoryRegistry.All().ToList();

                        var table = new ConsoleTable("Name", "ID");

                        foreach (var repo in repos)
                        {
                            table.AddRow(repo.Name, repo.RepoId);
                        }

                        if (repos.Any())
                        {
                            table.Print();
                        }
                        else
                        {
                            Console.WriteLine("You have no private repositories");
                        }

                        break;
                    }

                default:
                    throw new InvalidCommandParamaterException($"Invalid Repository action \"{repoActionsOptions.Action}\"", repoActionsOptions);
            }
        }
    }
}