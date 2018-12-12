using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        Task<PackageInfo> GetAsync(string packageId, string tag, string repository);
        Task<List<PackageInfo>> Search(string term, string repository = null, bool @select = false);
    }
}
