using System;
using SevenZip;

namespace AppGet.Installers.Inno
{
    public class InnoDetector : IDetectInstallMethod
    {
        public decimal GetConfidence(string path, SevenZipExtractor zip)
        {
            foreach (var prop in zip.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("INNO"))
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
