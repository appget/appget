using AppGet.Exceptions;

namespace AppGet.PackageRepository
{
    public class PackageNotFoundException : AppGetException
    {
        public PackageNotFoundException(string packageName)
            : base($"Package [{packageName}] could not be found")
        {
        }
    }
}