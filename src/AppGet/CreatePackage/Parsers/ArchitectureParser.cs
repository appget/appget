using System;
using System.Text.RegularExpressions;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Parsers
{
    public static class ArchitectureParser
    {
        private static readonly Regex[] ArchRegexes = {
            new Regex("x64", RegexOptions.IgnoreCase),
            new Regex("win64", RegexOptions.IgnoreCase),
            new Regex("64bit", RegexOptions.IgnoreCase),
            new Regex("\\D64\\b", RegexOptions.IgnoreCase)
        };

        public static ArchitectureTypes Parse(Uri uri)
        {
            var source = uri.LocalPath + uri.Query;

            foreach (var regex in ArchRegexes)
            {
                var match = regex.Match(source);
                if (match.Success)
                {
                    return ArchitectureTypes.x64;

                }
            }

            return ArchitectureTypes.x86;
        }

    }
}
