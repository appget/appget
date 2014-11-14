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
        string GetInstallerLogFile(PackageManifest packageManifest);
        string GetInstallationPath(PackageManifest packageManifest);
    }

    public class PathResolver : IPathResolver
    {
        public string TempFolder
        {
            get
            {
                return Path.GetTempPath();
            }
        }
        
        private string ProgramData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            }
        }

        public string AppDataDirectory
        {
            get { return Path.Combine(ProgramData, "AppGet"); }
        }

        public string InstalledPackageList
        {
            get { return Path.Combine(AppDataDirectory, "packages.yaml"); }
        }

        public string GetInstallerLogFile(PackageManifest packageManifest)
        {
            var installerLogDir = Path.Combine(AppDataDirectory, "Logs");

            Directory.CreateDirectory(installerLogDir);

            var fileName = string.Format("{0}_{1:yyyyMMdd_HHssmm}.log", packageManifest.Id, DateTime.Now);

            return Path.Combine(installerLogDir, fileName);
        }


        public string GetInstallationPath(PackageManifest packageManifest)
        {
            switch (packageManifest.InstallMethod)
            {
                case InstallMethodType.Zip:
                    {
                        var folderName = string.Format("{0}-{1}", packageManifest.Id, packageManifest.Version);
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