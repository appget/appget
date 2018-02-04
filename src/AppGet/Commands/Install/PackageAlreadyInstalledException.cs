using AppGet.Exceptions;

namespace AppGet.Commands.Install
{
    public class PackageAlreadyInstalledException : AppGetException
    {
        public PackageAlreadyInstalledException(string packageId)
            : base($"[{packageId}] is already installed.")
        {
        }
    }
}