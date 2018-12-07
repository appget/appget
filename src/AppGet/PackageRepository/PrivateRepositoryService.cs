using System;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Extensions;

namespace AppGet.PackageRepository
{
    public class PrivateRepositoryService
    {
        private readonly NexClient _nexClient;
        private readonly RepositoryRegistry _repositoryRegistry;

        public PrivateRepositoryService(NexClient nexClient, RepositoryRegistry repositoryRegistry)
        {
            _nexClient = nexClient;
            _repositoryRegistry = repositoryRegistry;
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
    }
}
