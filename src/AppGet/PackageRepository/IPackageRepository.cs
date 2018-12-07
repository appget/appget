using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        Task<PackageInfo> GetAsync(string packageId, string tag, string repository);
    }
}
