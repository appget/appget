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

        public PackageInfo Get(string id, string tag)
        {
            var task = _repositories.Select(c => c.Get(id, tag)).FirstOrDefault(c => c != null);

            return task == null ? null : task;
        }

        public List<PackageInfo> Search(string term, bool select = false)
        {
            var task = _repositories.SelectMany(c => c.Search(term)).ToList();

            return task;
        }
    }
}