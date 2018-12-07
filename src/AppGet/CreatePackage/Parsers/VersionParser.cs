using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppGet.CreatePackage.Parsers
{
    public static class VersionParser
    {
        private static readonly Regex[] VersionRegexes =
        {
            new Regex(@"\d+(\.\d+){1,3}", RegexOptions.Compiled),
            new Regex(@"\d{2,4}", RegexOptions.Compiled)
        };

        private static readonly Regex[] KnownNumberCleanup =
        {
            new Regex(@"(\w|\b)(ia|x|win)(64|32)(\w|\b)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"(\w|\b)i?(x|\d)86(\w|\b)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"(\w|\b)win(dows)?.?\d{1,4}(\w|\b)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"id=\d{2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled)
        };

        public static string Parse(string text)
        {
            return ParseAll(text).OrderByDescending(c => c.Split('.').Length).ThenByDescending(c => c.Length).FirstOrDefault();
        }

        public static IEnumerable<string> ParseAll(string text)
        {
            foreach (var regex in KnownNumberCleanup)
            {
                text = regex.Replace(text, "");
            }

            return VersionRegexes.SelectMany(r => r.Matches(text).Cast<Capture>().Select(c => c.Value)).Where(v => v != "32" && v != "64");
        }

        public static string Parse(Uri uri)
        {
            var source = uri.LocalPath + uri.Query;

            return Parse(source);
        }
    }
}