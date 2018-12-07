using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        Task<PackageInfo> GetAsync(string id, string tag);
        Task<List<PackageInfo>> Search(string term, bool select = false);
    }
}