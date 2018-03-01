using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        Task<PackageInfo> Get(string id, string tag);
        Task<List<PackageInfo>> Search(string term);
    }
}