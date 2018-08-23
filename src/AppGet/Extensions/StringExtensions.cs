using System.Text;
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

        public static string OrBlank(this string value)
        {
            if (value == null) return string.Empty;
            return value;
        }

        public static string JoinTo(this string value, params string[] others)
        {
            var builder = new StringBuilder(value);
            foreach (var v in others)
            {
                builder.Append(v);
            }
            return builder.ToString();
        }

        public static StringBuilder AppendWhen(this StringBuilder builder, bool condition, params string[] values)
        {
            if (condition)
                foreach (var value in values)
                    builder.Append(value);

            return builder;
        }

        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string ifTrue, string ifFalse)
        {
            return condition
                ? builder.Append(ifTrue)
                : builder.Append(ifFalse);
        }
    }
}