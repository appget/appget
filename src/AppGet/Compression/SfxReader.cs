using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SevenZip;

namespace AppGet.Compression
{
    public interface ISfxReader
    {
        XDocument Read(SevenZipExtractor archive);
        string ReadRaw(SevenZipExtractor archive);
    }

    public class SfxReader : ISfxReader
    {
        private static readonly Regex AssemblyRegex = new Regex(@"\<(\w+:)?assembly\W(.|\n)*assembly>", RegexOptions.Compiled);

        public XDocument Read(SevenZipExtractor archive)
        {
            var rawText = ReadRaw(archive);
            var assemblyElementText = AssemblyRegex.Match(rawText).Value;
            var xDocument = XDocument.Parse("<?xml version=\"1.0\" standalone=\"yes\"?> " + assemblyElementText);
            return xDocument;
        }

        public string ReadRaw(SevenZipExtractor archive)
        {
            var entry = archive.ArchiveFileData.First(e => e.FileName.EndsWith("MANIFEST\\1", StringComparison.InvariantCultureIgnoreCase));
            using (var memoryStream = new MemoryStream())
            {
                archive.ExtractFile(entry.Index, memoryStream);
                return Encoding.ASCII.GetString(memoryStream.ToArray());
            }
        }
    }
}
