using AppGet.Exceptions;

namespace AppGet.Commands.Install
{
    public class PackageAlreadyInstalledException : AppGetException
    {
        public PackageAlreadyInstalledException(string packageId)
            : base("[{0}] is already installed.", packageId)
        {
        }
    }
}