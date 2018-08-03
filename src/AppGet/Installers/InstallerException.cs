using System;
using AppGet.Exceptions;
using AppGet.Manifest;

namespace AppGet.Installers
{
    public class InstallerException : AppGetException
    {
        public PackageManifest PackageManifest { get; }
        public string LogPath { get; }
        public ExistReason ExitReason { get; }
        public int ExitCode{ get; }

        public InstallerException(int exitCode, PackageManifest packageManifest, ExistReason exitReason, string logPath)
            : base(GetMessage(exitCode, packageManifest, exitReason, logPath))
        {
            PackageManifest = packageManifest;
            LogPath = logPath;
            ExitReason = exitReason;
            ExitCode = exitCode;
        }

        private static string GetMessage(int exitCode, PackageManifest packageManifest, ExistReason existReason, string logPath)
        {
            var msg = $"Installer for {packageManifest.Name} {packageManifest.Version} returned with a non-zero exit code: {exitCode}.";

            if (existReason != null)
            {
                msg += $" {existReason.Message}";
            }

            if (!string.IsNullOrWhiteSpace(logPath))
            {
                msg += Environment.NewLine + $"Logs: {logPath}";
            }

            return msg;
        }
    }
}