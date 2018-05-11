using System.Collections.Generic;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        PackageInfo Get(string id, string tag);
        List<PackageInfo> Search(string term, bool select = false);
    }
}