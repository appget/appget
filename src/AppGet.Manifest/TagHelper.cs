using System;
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

        private static string ParseTarget(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input can't be empty or blank", nameof(input));
            }

            input = input.ToLowerInvariant();

            if (input.IndexOfAny(new[] { '\\' }) >= 0 || input.EndsWith(".yaml"))
            {
                return Path.GetFileNameWithoutExtension(input).ToLowerInvariant();
            }

            return input.Trim();
        }

        public static string ParseTag(string input)
        {
            var target = ParseTarget(input);
            var indexOfTag = IndexOfTag(target);

            if (indexOfTag > 0)
            {
                return target.Substring(indexOfTag + 1).Trim(TrimChars);
            }

            return null;
        }

        public static string ParseId(string input)
        {
            var target = ParseTarget(input);
            var indexOfTag = IndexOfTag(target);

            if (indexOfTag > 0)
            {
                return target.Substring(0, indexOfTag).Trim(TrimChars);
            }

            return target;
        }
    }
}
