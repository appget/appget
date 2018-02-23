using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        Task<PackageInfo> GetLatest(string name);
        Task<List<PackageInfo>> Search(string term);
    }
}