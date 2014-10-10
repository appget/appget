using System;
using System.IO;
using AppGet.FlightPlans;

namespace AppGet.HostSystem
{
    public interface IPathResolver
    {
        string GetInstallerTempPath(string fileName);
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
            switch (flightPlan.Installer)
            {
                case InstallerType.Zip:
                    {
                        return Path.Combine(ProgramData, flightPlan.Id + flightPlan.Version);
                    }
                default:
                    {
                        throw new NotImplementedException(flightPlan.Installer + " is not supported yet.");
                    }
            }
        }
    }
}