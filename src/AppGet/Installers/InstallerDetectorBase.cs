using System.Linq;
using AppGet.CreatePackage;
using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers
{
    public abstract class InstallerDetectorBase : IDetectInstallMethod
    {
        public abstract InstallMethodTypes InstallMethod { get; }

        protected virtual string[] Terms => new[] { InstallMethod.ToString().ToLowerInvariant() };

        public virtual Confidence GetConfidence(string path, SevenZipExtractor archive, string exeManifest)
        {
            return Terms.Any(t => HasProperty(archive, t) || ManifestContains(exeManifest, t)) ? Confidence.VeryHigh : Confidence.NoMatch;
        }

        private static bool HasProperty(SevenZipExtractor archive, string term)
        {
            if (archive == null) return false;

            term = term.ToLowerInvariant().Trim();

            foreach (var prop in archive.ArchiveProperties)
            {
                var propertyText = $"{prop.Name} {prop.Value}".ToLowerInvariant();
                if (propertyText.Contains(term))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ManifestContains(string exeManifest, string term)
        {
            if (string.IsNullOrWhiteSpace(exeManifest)) return false;

            return exeManifest.ToLowerInvariant().Contains(term.ToLowerInvariant());
        }
    }
}