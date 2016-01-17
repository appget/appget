using System;

namespace AppGet.PackageRepository
{
    public class PackageInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string MajorVersion { get; set; }
        public string ManifestUrl { get; set; }
        public string SourceUrl { get; set; }

        public override string ToString()
        {
            var formatted = $"[{Id}] {Name}";

            if (!String.IsNullOrEmpty(Version))
            {
                formatted += $" ({Version})";
            }

            return formatted;
        }
    }
}