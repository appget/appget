using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGet.PackageRepository
{
    public class AggregateRepository : IPackageRepository
    {
        private readonly IEnumerable<IPackageRepository> _repositories;

        public AggregateRepository(IEnumerable<IPackageRepository> repositories)
        {
            _repositories = repositories;
        }

        public Task<PackageInfo> GetLatest(string name)
        {
            return _repositories.Select(c => c.GetLatest(name)).FirstOrDefault();
        }

        public Task<List<PackageInfo>> Search(string term)
        {
            return Task.FromResult(_repositories.SelectMany(c => c.Search(term).Result).ToList());
        }
    }
}