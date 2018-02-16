using SevenZip;

namespace AppGet.Installers.Msi
{
    public class MsiDetector : IDetectInstallMethod
    {
        public decimal GetConfidence(string path, SevenZipExtractor zip)
        {
            if (path.ToLowerInvariant().EndsWith(".msi"))
            {
                return 1;
            }

            if (zip.ArchiveFileNames.Contains(".wixburn"))
            {
                return 1;
            }

            foreach (var prop in zip.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("MSI"))
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
