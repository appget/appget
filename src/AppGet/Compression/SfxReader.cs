using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SevenZip;

namespace AppGet.Compression
{
    public class SfxReader
    {
        private static readonly Regex AssemblyRegex = new Regex(@"\<(\w+:)?assembly\W(.|\n)*assembly>", RegexOptions.Compiled);

        public XDocument Read(string path)
        {
            using (var archiveFile = new SevenZipExtractor(path, InArchiveFormat.PE))
            {
                var entry = archiveFile.ArchiveFileData.First(e => e.FileName.EndsWith("MANIFEST\\1", StringComparison.InvariantCultureIgnoreCase));
                Console.WriteLine(entry.FileName);
                // extract to stream
                using (var memoryStream = new MemoryStream())
                {
                    archiveFile.ExtractFile(entry.Index, memoryStream);
                    var text = Encoding.ASCII.GetString(memoryStream.ToArray());
                    var assemblyElementText = AssemblyRegex.Match(text).Value;
                    var xDocument = XDocument.Parse("<?xml version=\"1.0\" standalone=\"yes\"?> " + assemblyElementText);
                    return xDocument;
                }
            }
        }
    }
}
