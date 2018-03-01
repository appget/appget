using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppGet.CreatePackage.Parsers
{
    public static class HomepageParser
    {
        private static readonly Regex HostNameRegex = new Regex(@"^www\w*", RegexOptions.IgnoreCase);

        private static readonly string[] CDNs = {
            "amazonaws",
            "fosshub",
            "akamaihd",
            "code42",
            "netdna-ssl",
            "github.com",
            "sourceforge"
        };


        public static string Parse(Uri uri)
        {
            if (CDNs.Any(c => uri.Host.ToLowerInvariant().Contains(c)))
            {
                return null;
            }

            var domainParts = uri.Host.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var subDomainsCount = Math.Max(0, domainParts.Length - 2);
            var subs = domainParts.Take(subDomainsCount).ToList();
            var host = domainParts.Skip(subDomainsCount).ToList();

            for (var i = subs.Count; i > 0; i--)
            {
                if (HostNameRegex.IsMatch(subs[i - 1]))
                {
                    host.Insert(0, subs[i - 1]);
                }
                else
                {
                    break;
                }
            }

            return $"{uri.Scheme}://{string.Join(".", host)}";
        }
    }
}