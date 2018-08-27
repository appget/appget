using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace AppGet.HostSystem
{
    public interface IEnvInfo
    {
        Version WindowsVersion { get; }
        string Name { get; }
        string FullName { get; }
        bool Is64BitOperatingSystem { get; }
        bool UserInteractive { get; }
        string AppDir { get; }
        bool IsAdministrator { get; }
    }

    public class EnvInfo : IEnvInfo
    {
        public Version WindowsVersion { get; }
        public string Name { get; }
        public string FullName { get; }
        public bool Is64BitOperatingSystem { get; }
        public bool UserInteractive => Environment.UserInteractive;
        public bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        public string AppDir { get; }


        public bool IsGui { get; }

        [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
        private static extern bool IsOS(int os);

        private static bool IsWindowsServer()
        {
            const int OS_ANYSERVER = 29;

            return IsOS(OS_ANYSERVER);
        }

        public EnvInfo()
        {

            IsGui = Process.GetCurrentProcess().ProcessName.Contains("gui");
            try
            {
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
                var windowsServer = IsWindowsServer();
                Name = windowsServer ? "Windows Server" : "Windows";
                WindowsVersion = Environment.OSVersion.Version;

                AppDir = Path.GetDirectoryName(GetType().Assembly.Location);

                if (string.IsNullOrWhiteSpace(Environment.OSVersion.VersionString))
                {
                    FullName = $"{Name} {WindowsVersion}";
                }

                FullName = Environment.OSVersion.VersionString;
            }
            catch (Exception)
            {
            }
        }
    }
}