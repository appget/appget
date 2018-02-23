using System;
using System.Runtime.InteropServices;
using NLog;

namespace AppGet.HostSystem
{
    public interface IOsInfo
    {
        Version Version { get; }
        string Name { get; }
        string FullName { get; }
        bool Is64BitOperatingSystem { get; }
    }

    public class OsInfo : IOsInfo
    {
        public Version Version { get; }
        public string Name { get; }
        public string FullName { get; }
        public bool Is64BitOperatingSystem { get; }

        [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
        private static extern bool IsOS(int os);

        private static bool IsWindowsServer()
        {
            const int OS_ANYSERVER = 29;
            return IsOS(OS_ANYSERVER);
        }

        public OsInfo()
        {
            try
            {
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
                var windowsServer = IsWindowsServer();
                Name = windowsServer ? "Windows Server" : "Windows";
                Version = Environment.OSVersion.Version;
                if (string.IsNullOrWhiteSpace(Environment.OSVersion.VersionString))
                {
                    FullName = $"{Name} {Version}";
                }

                FullName = Environment.OSVersion.VersionString;
            }
            catch (Exception e)
            {
            }
        }
    }
}