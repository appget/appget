using System.Collections.Generic;

namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        PackageInfo GetLatest(string name);
        List<PackageInfo> Search(string term);
    }
}