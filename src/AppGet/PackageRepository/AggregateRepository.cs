using System.Collections.Generic;
using System.Linq;

namespace AppGet.PackageRepository
{
    public class AggregateRepository : IPackageRepository
    {
        private readonly IEnumerable<IPackageRepository> _repositories;

        public AggregateRepository(IEnumerable<IPackageRepository> repositories)
        {
            _repositories = repositories;
        }

        public PackageInfo GetLatest(string name)
        {
            return _repositories.Select(c => c.GetLatest(name)).FirstOrDefault();
        }

        public List<PackageInfo> Search(string term)
        {
            return _repositories.SelectMany(c => c.Search(term)).ToList();
        }
    }
}