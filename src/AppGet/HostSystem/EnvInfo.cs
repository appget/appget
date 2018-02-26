using System;
using System.Runtime.InteropServices;

namespace AppGet.HostSystem
{
    public interface IEnvInfo
    {
        Version Version { get; }
        string Name { get; }
        string FullName { get; }
        bool Is64BitOperatingSystem { get; }
        bool UserInteractive { get; }
    }

    public class EnvInfo : IEnvInfo
    {
        public Version Version { get; }
        public string Name { get; }
        public string FullName { get; }
        public bool Is64BitOperatingSystem { get; }
        public bool UserInteractive => Environment.UserInteractive;

        [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
        private static extern bool IsOS(int os);

        private static bool IsWindowsServer()
        {
            const int OS_ANYSERVER = 29;
            return IsOS(OS_ANYSERVER);
        }

        public EnvInfo()
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