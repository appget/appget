using System;
using System.IO;
using AppGet.FlightPlans;

namespace AppGet.HostSystem
{
    public interface IPathResolver
    {
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
                        var appGetFolder = Path.Combine(ProgramData, "AppGet");
                        var folderName = string.Format("{0}-{1}", flightPlan.Id, flightPlan.Version);
                        return Path.Combine(appGetFolder, folderName);
                    }
                default:
                    {
                        throw new NotImplementedException(flightPlan.InstallMethod + " is not supported yet.");
                    }
            }
        }
    }
}