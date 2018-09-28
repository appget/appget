using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AppGet.HostSystem
{
    public static class BuildInfo
    {
        static BuildInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            AppVersion = new Version(fvi.ProductVersion);
            ProductName = fvi.ProductName;
        }

        public static Version AppVersion { get; }
        public static string ProductName { get; }

        public static DateTime BuildDateTime
        {
            get
            {
                var fileLocation = Assembly.GetCallingAssembly().Location;

                return new FileInfo(fileLocation).LastWriteTimeUtc;
            }
        }

        public static bool IsProduction
        {
            get
            {
                if (IsDebug) return false;

                if (AppVersion == new Version("1.0.0.0"))
                {
                    return false;
                }

                return true;
            }
        }

        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}