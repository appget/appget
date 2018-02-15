using System;
using System.IO;
using AppGet.Manifests;

namespace AppGet.HostSystem
{
    public interface IPathResolver
    {
        string AppDataDirectory { get; }
        string InstalledPackageList { get; }
        string TempFolder { get; }
        string GetInstallerLogFile(string packageId);
        string GetInstallationPath(PackageManifest packageManifest);
    }

    public class PathResolver : IPathResolver
    {
        public PathResolver()
        {
            Directory.CreateDirectory(TempFolder);
            Directory.CreateDirectory(AppDataDirectory);
        }

        public string TempFolder => Path.Combine(AppDataDirectory, "Temp");

        private string ProgramData => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public string AppDataDirectory => Path.Combine(ProgramData, "AppGet");

        public string InstalledPackageList => Path.Combine(AppDataDirectory, "packages.yaml");

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