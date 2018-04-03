using System.Collections.Generic;
using AppGet.Exceptions;

namespace AppGet.PackageRepository
{
    public class PackageNotFoundException : AppGetException
    {
        public List<PackageInfo> Similar { get; }

        public PackageNotFoundException(string packageId, string tag, List<PackageInfo> similar)
            : base($"Package [{packageId}] could not be found")
        {
            Similar = similar;
        }
    }
}