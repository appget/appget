using System.Text.RegularExpressions;

namespace AppGet.Manifest
{
    public static class IdHelper
    {
        private static readonly Regex IdRegex = new Regex(@"[\W,_]+", RegexOptions.Compiled);

        public static string ToPackageId(this string name)
        {
            name = name.Replace("+", "-plus-");
            var id = IdRegex.Replace(name, "-").ToLowerInvariant().Trim('-');
            return id;
        }
    }
}
