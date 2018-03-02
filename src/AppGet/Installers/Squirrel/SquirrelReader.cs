using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers.Zip;

namespace AppGet.Installers.Squirrel
{
    public interface ISquirrelReader
    {
        bool IsSquirrel(SevenZipExtractor archive);
        NugetSpec GetNugetSpec(SevenZipExtractor archive);
    }

    public class SquirrelReader : ISquirrelReader
    {
        private static EntryStream Open(Stream stream, string endsWith)
        {
            var reader = ZipReader.Open(stream);

            while (reader.MoveToNextEntry())
            {
                if (reader.Entry.Key.EndsWith(endsWith))
                {
                    return reader.OpenEntryStream();
                }
            }

            return null;
        }

        private static ArchiveFileInfo? GetEmbededArchive(SevenZipExtractor archive)
        {
            var biggestFile = archive.ArchiveFileData.OrderByDescending(c => c.Size).First();

            if (!biggestFile.FileName.EndsWith("DATA\\131"))
            {
                return null;
            }

            return biggestFile;
        }

        public bool IsSquirrel(SevenZipExtractor archive)
        {
            var embededArchive = GetEmbededArchive(archive);
            if (embededArchive == null) return false;

            using (var memoryStream = new MemoryStream())
            {
                archive.ExtractFile(embededArchive.Value.FileName, memoryStream);
                memoryStream.Position = 0;

                using (var nugetStream = Open(memoryStream, ".nupkg"))
                {
                    return nugetStream != null;
                }
            }
        }

        public NugetSpec GetNugetSpec(SevenZipExtractor archive)
        {
            var embededArchive = GetEmbededArchive(archive);
            if (embededArchive == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                archive.ExtractFile(embededArchive.Value.FileName, memoryStream);
                memoryStream.Position = 0;

                using (var nugetStream = Open(memoryStream, ".nupkg"))
                {
                    if (nugetStream == null) return null;

                    using (var manifestStream = Open(nugetStream, ".nuspec"))
                    {
                        if (manifestStream == null) return null;

                        using (var reader = new XmlTextReader(manifestStream) { Namespaces = false })
                        {
                            var serializer = new XmlSerializer(typeof(NugetSpec));
                            return (NugetSpec)serializer.Deserialize(reader);
                        }
                    }
                }
            }
        }
    }
}
