using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using AppGet.Manifests;
using SevenZip;
using SharpCompress.Common.Zip;
using SharpCompress.Readers.Zip;

namespace AppGet.Installers.Squirrel
{
    public class SquirrelDetector : IDetectInstallMethod
    {
        private readonly ISfxReader _sfxReader;
        public InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;


        public SquirrelDetector(ISfxReader sfxReader)
        {
            _sfxReader = sfxReader;
        }


        private IEnumerable<ZipEntry> GetInternal(SevenZipExtractor archive)
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

        public decimal GetConfidence(string path, SevenZipExtractor archive)
        {
            foreach (var names in archive.ArchiveFileNames)
            {
                Console.WriteLine(names);
            }


            foreach (var prop in archive.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("SQUIRREL"))
                {
                    return 1;
                }

                if (prop.Name != null && prop.Name.ToLowerInvariant().Contains("squirrel"))
                {
                    return 1;
                }
            }

            var entries = GetInternal(archive);

            if (entries.Any(c => c.Key.EndsWith(".nupkg")))
            {
                return 1;
            }



            return 0;
            //            return raw.ToLowerInvariant().Contains("squirrel") ? 1 : 0;
        }
    }
}
