namespace AppGet.Extensions
{
    public static class NumeralExtensions
    {
        public static string ToFileSize(this long bytes)
        {
            const string SUFFIX = "MB";
            double readable = bytes >> 10;

            // Divide by 1024 to get fractional value
            readable = readable / 1024;
            // Return formatted number with suffix
            return $"{readable:N0} {SUFFIX}";
        }
    }
}
