using System.IO;

namespace AppGet.Manifest
{
    public static class TagHelper
    {

        private static readonly char[] TrimChars = { '.', ':', '@' };

        private static int IndexOfTag(string text)
        {
            return text?.IndexOfAny(new[] { '_', ':', '@' }) ?? -1;
        }

        private static string ParseTarget(string text)
        {
            if (text.IndexOfAny(new[] { '\\' }) >= 0 || text.EndsWith(".yaml"))
            {
                return Path.GetFileNameWithoutExtension(text).ToLowerInvariant();
            }

            return text.Trim();
        }

        public static string ParseTag(string text)
        {
            var target = ParseTarget(text);
            var indexOfTag = IndexOfTag(target);

            if (indexOfTag > 0)
            {
                return target.Substring(indexOfTag + 1).Trim(TrimChars);
            }

            return null;
        }

        public static string ParseId(string text)
        {
            var target = ParseTarget(text);
            var indexOfTag = IndexOfTag(target);

            if (indexOfTag > 0)
            {
                return target.Substring(0, indexOfTag).Trim(TrimChars);
            }

            return target;
        }
    }
}
