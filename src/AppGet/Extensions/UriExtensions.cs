using System;
using System.Text.RegularExpressions;

namespace AppGet.Extensions
{
    public static class UriExtensions
    {
        private static readonly Regex HttpsRegex = new Regex("^http:\\/\\/\\b", RegexOptions.IgnoreCase);

        public static Uri ToHttps(this Uri uri)
        {
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                return uri;
            }

            var httpsUrl = HttpsRegex.Replace(uri.ToString(), "https://");
            return new Uri(httpsUrl);
        }

    }
}