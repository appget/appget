using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Manifests;
using SevenZip;
using SharpCompress.Common.Zip;
using SharpCompress.Readers.Zip;

namespace AppGet.Installers.Squirrel
{
    public class SquirrelDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;


        private static IEnumerable<ZipEntry> GetInternal(SevenZipExtractor archive)
        {
            var biggestFile = archive.ArchiveFileData.OrderByDescending(c => c.Size).First();

            if (!biggestFile.FileName.EndsWith("DATA\\131"))
            {
                yield break;
            }

            using (var memoryStream = new MemoryStream())
            {
                archive.ExtractFile(biggestFile.FileName, memoryStream);
                memoryStream.Position = 0;

                var reader = ZipReader.Open(memoryStream);

                while (reader.MoveToNextEntry())
                {
                    yield return reader.Entry;
                }
            }

        }

        public override decimal GetConfidence(string path, SevenZipExtractor archive, string exeManifest)
        {
            if (base.GetConfidence(path, archive, exeManifest) == 1)
            {
                return 1;
            }

            if (archive != null)
            {
                var entries = GetInternal(archive);

                return entries.Any(c => c.Key.EndsWith(".nupkg")) ? 1 : 0;
            }

            return 0;
        }
    }
}
