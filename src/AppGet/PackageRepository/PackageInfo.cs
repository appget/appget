using AppGet.Exceptions;

namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string FlightPlanUrl { get; set; }
        public string SourceUrl { get; set; }
    }


    public class PackageNotFoundException : AppGetException
    {
        public PackageNotFoundException(string packageName)
            : base("Package [{0}] could not be found", packageName)
        {
        }


    }
}