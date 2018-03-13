using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppGet.Extensions
{
    public static class StringExtensions
    {
        private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

        private static readonly Regex AlphanumericRegex = new Regex("[\\W_]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value?.Trim());
        }

        public static string ToAlphaNumeric(this string value)
        {
            return AlphanumericRegex.Replace(value, "").ToLowerInvariant();
        }

        public static string ToTitleCase(this string value)
        {
            return TextInfo.ToTitleCase(value);
        }

        public static int CapitalLettersCount(this string value)
        {
            return value.Count(char.IsUpper);
        }

        public static int PeriodCount(this string value)
        {
            return value.Count(c => c == '.');
        }
    }
}