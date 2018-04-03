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

        public async Task<PackageInfo> Get(string id, string tag)
        {
            var task = _repositories.Select(c => c.Get(id, tag)).FirstOrDefault(c => c != null);
            return task == null ? null : await task;
        }

        public async Task<List<PackageInfo>> Search(string term)
        {
            var task = Task.FromResult(_repositories.SelectMany(c => c.Search(term).Result).ToList());
            return await task;
        }
    }
}