using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppGet.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex AlphanumericRegex = new Regex("[\\W_]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value?.Trim());
        }

        public static string ToAlphaNumeric(this string value)
        {
            return AlphanumericRegex.Replace(value, "").ToLowerInvariant();
        }
    }
}