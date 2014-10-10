namespace AppGet.PackageRepository
{
    public interface IPackageRepository
    {
        PackageInfo FindPackage(string name);
    }
}