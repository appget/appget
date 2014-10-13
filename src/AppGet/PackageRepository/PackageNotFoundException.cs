using AppGet.Exceptions;

namespace AppGet.PackageRepository
{
    public class PackageNotFoundException : AppGetException
    {
        public PackageNotFoundException(string packageName)
            : base("Package [{0}] could not be found", packageName)
        {
        }


    }
}