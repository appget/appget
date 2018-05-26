using System.Collections.Generic;
using AppGet.Exceptions;
using AppGet.Manifest;
using AppGet.Manifests;

namespace AppGet.PackageRepository
{
    public class PackageNotFoundException : AppGetException
    {
        public List<PackageInfo> Similar { get; }

        public PackageNotFoundException(string packageId, string tag, List<PackageInfo> similar)
            : base($"Package [{packageId}:{tag ?? PackageManifest.LATEST_TAG}] could not be found")
        {
            Similar = similar;
        }
    }
}