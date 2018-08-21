using System.Collections.Generic;
using AppGet.Exceptions;
using AppGet.Manifest;

namespace AppGet.PackageRepository
{
    public class PackageNotFoundException : AppGetException
    {
        public string PackageId { get; }
        public string Tag { get; }
        public List<PackageInfo> Similar { get; }

        public PackageNotFoundException(string packageId, string tag, List<PackageInfo> similar)
            : base($"Package [{packageId}:{tag ?? PackageManifest.LATEST_TAG}] could not be found")
        {
            PackageId = packageId;
            Tag = tag;
            Similar = similar;
        }
    }
}