using SevenZip;

namespace AppGet.Installers
{
    public class InstallerDetectorBase
    {
        protected static bool HasProperty(SevenZipExtractor archive, string term)
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
    }
}