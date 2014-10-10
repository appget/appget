namespace AppGet.PackageProvider
{
    public interface IPackageProvider
    {
        PackageInfo FindPackage(string name);
    }
}