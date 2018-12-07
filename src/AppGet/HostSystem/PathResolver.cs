using System;
using System.IO;
using AppGet.Manifest;

namespace AppGet.HostSystem
{
    public interface IPathResolver
    {
        string TempFolder { get; }
        string InstallerCacheFolder { get; }
        string AppDataDirectory { get; }
        string GetInstallerLogFile(string packageId);
        string GetInstallationPath(PackageManifest packageManifest);
    }

    public class PathResolver : IPathResolver
    {
        public PathResolver()
        {
            Directory.CreateDirectory(TempFolder);
            Directory.CreateDirectory(AppDataDirectory);
            Directory.CreateDirectory(InstallerCacheFolder);
        }

        public string TempFolder => Path.Combine(AppDataDirectory, "Temp");
        public string InstallerCacheFolder => Path.Combine(AppDataDirectory, "InstallerCache");

        private string ProgramData => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public string AppDataDirectory => Path.Combine(ProgramData, "AppGet");

        public string GetInstallerLogFile(string packageId)
        {
            var installerLogDir = Path.Combine(AppDataDirectory, "Logs");

            Directory.CreateDirectory(installerLogDir);

            var fileName = $"{packageId}_{DateTime.Now:yyyyMMdd_HHmmss}.log".ToLowerInvariant();

            return Path.Combine(installerLogDir, fileName);
        }

        public string GetInstallationPath(PackageManifest packageManifest)
        {
            switch (packageManifest.InstallMethod)
            {
                case InstallMethodTypes.Zip:
                {
                    var folderName = $"{packageManifest.Id}-{packageManifest.Version}";

                    return Path.Combine(AppDataDirectory, folderName);
                }
                default:
                {
                    throw new NotImplementedException(packageManifest.InstallMethod + " is not supported yet.");
                }
            }
        }
    }
}