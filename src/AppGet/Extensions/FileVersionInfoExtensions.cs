using System.Diagnostics;
using System.Linq;

namespace AppGet.Extensions
{
    public static class FileVersionInfoExtensions
    {
        public static string ParseProductName(this FileVersionInfo fileVersionInfo)
        {
            if (fileVersionInfo != null)
            {
                return new[]
                    {
                        fileVersionInfo.ProductName,
                        fileVersionInfo.InternalName,
                        fileVersionInfo.CompanyName
                    }.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
                    ?.Trim();
            }

            return null;
        }
    }
}