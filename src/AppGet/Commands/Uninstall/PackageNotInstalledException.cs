using AppGet.Exceptions;

namespace AppGet.Commands.Uninstall
{
    public class PackageNotInstalledException : AppGetException
    {
        public PackageNotInstalledException(string packageName)
            : base("Package [{0}] is not currently installed", packageName)
        {
        }
    }
}