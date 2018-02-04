using AppGet.Exceptions;

namespace AppGet.Commands.Uninstall
{
    public class PackageNotInstalledException : AppGetException
    {
        public PackageNotInstalledException(string packageName)
            : base($"Package [{packageName}] is not currently installed")
        {
        }
    }
}