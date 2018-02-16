using SevenZip;

namespace AppGet.Installers.Nsis
{
    public class NsisDetector : IDetectInstallMethod
    {
        public decimal GetConfidence(string path, SevenZipExtractor zip)
        {
            foreach (var prop in zip.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("NSIS"))
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
