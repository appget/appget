using System;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Extensions;
using NLog;

namespace AppGet.PackageRepository
{
    public class PrivateRepositoryService
    {
        private readonly NexClient _nexClient;
        private readonly RepositoryRegistry _repositoryRegistry;
        private readonly Logger _logger;

        public PrivateRepositoryService(NexClient nexClient, RepositoryRegistry repositoryRegistry, Logger logger)
        {
            _nexClient = nexClient;
            _repositoryRegistry = repositoryRegistry;
            _logger = logger;
        }


        public async Task AddRepository(string uri)
        {
            var repoConnection = new Uri(uri);

            var repoId = repoConnection.UserInfo.Split(':').First();
            var key = repoConnection.UserInfo.Split(':').Last();

            if (repoId.IsNullOrWhiteSpace())
            {
                throw new AddRepositoryException("Missing or invalid ID.");
            }

            if (repoId.IsNullOrWhiteSpace())
            {
                throw new AddRepositoryException("Missing or invalid Key.");
            }


            var repo = await _nexClient.AuthenticateRepository(repoId, key);

            _repositoryRegistry.AddRepo(repo);
        }

        public async Task RemoveRepository(string name, string id)
        {
            var repos = _repositoryRegistry.All();

            var repo = repos.First(c => c.RepoId == id || c.Name == name);

            if (repo == null)
            {
                throw new RepositoryNotFoundException(name, id);
            }

            _repositoryRegistry.Remove(repo.Name);

            _logger.Info($"Removed {repo.Name}:{repo.RepoId}");

        }
    }
}
