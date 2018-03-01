using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppGet.CreatePackage.Parsers
{
    public static class VersionParser
    {
        private static readonly Regex[] VersionRegexes = {
            new Regex(@"\d+(\.\d+){1,3}"),
            new Regex(@"\d{1,4}")

        };

        private static readonly Regex[] KnownNumberCleanup = {
            new Regex(@"(\w|\b)(ia|x|win)(64|32)(\w|\b)", RegexOptions.IgnoreCase),
            new Regex(@"(\w|\b)i?(x|\d)86(\w|\b)", RegexOptions.IgnoreCase),
            new Regex(@"(\w|\b)win(dows)?.?\d{1,4}(\w|\b)", RegexOptions.IgnoreCase),
        };

        public static string Parse(Uri uri)
        {
            var source = uri.LocalPath + uri.Query;

            foreach (var regex in KnownNumberCleanup)
            {
                source = regex.Replace(source, "");
            }

            foreach (var regex in VersionRegexes)
            {
                var matches = regex.Matches(source);
                if (matches.Count > 0)
                {
                    return matches.Cast<Capture>()
                        .OrderByDescending(c => c.Value.Length)
                        .First().Value;
                }
            }

            return null;
        }

    }
}