using System;
using AppGet.Manifest.Builder;

namespace AppGet.Manifest
{
    public static class PackageManifestExt
    {
        public static Uri Uri(this Installer installer)
        {
            return new Uri(installer.Location);
        }

        public static Uri Uri(this InstallerBuilder installer)
        {
            return new Uri(installer.Location);
        }
    }
}