using System;
using System.IO;
using AppGet.FlightPlans;

namespace AppGet.HostSystem
{
    public interface IPathResolver
    {
        string InstalledPackageList { get; }
        string GetInstallerTempPath(string fileName);
        string GetInstallationPath(FlightPlan flightPlan);
    }

    public class PathResolver : IPathResolver
    {
        private string TempFolder
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

        private string AppGetWrokingDirectory
        {
            get { return Path.Combine(ProgramData, "AppGet"); }
        }

        public string InstalledPackageList
        {
            get { return Path.Combine(AppGetWrokingDirectory, "packages.yaml"); }
        }

        public string GetInstallerTempPath(string fileName)
        {
            return Path.Combine(TempFolder, fileName);
        }


        public string GetInstallationPath(FlightPlan flightPlan)
        {
            switch (flightPlan.InstallMethod)
            {
                case InstallMethodType.Zip:
                    {
                        var folderName = string.Format("{0}-{1}", flightPlan.Id, flightPlan.Version);
                        return Path.Combine(AppGetWrokingDirectory, folderName);
                    }
                default:
                    {
                        throw new NotImplementedException(flightPlan.InstallMethod + " is not supported yet.");
                    }
            }
        }
    }
}