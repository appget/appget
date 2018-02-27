using System;
using System.Diagnostics;
using AppGet.Exceptions;
using AppGet.Manifests;

namespace AppGet.Installers
{
    public class InstallerException : AppGetException
    {
        public Process InstallerProcess { get; }
        public PackageManifest PackageManifest { get; }
        public string LogPath { get; }


        public InstallerException(Process installerProcess, PackageManifest packageManifest, string logPath)
            : base(GetMessage(installerProcess, packageManifest, logPath))
        {
            InstallerProcess = installerProcess;
            PackageManifest = packageManifest;
            LogPath = logPath;
        }

        private static string GetMessage(Process installerProcess, PackageManifest packageManifest, string logPath)
        {
            var msg = $"Installer for {packageManifest.Name} {packageManifest.Version} returned with a non-zero exit code: {installerProcess.ExitCode}";


            if (!string.IsNullOrWhiteSpace(logPath))
            {
                msg += Environment.NewLine + $"Logs: {logPath}";
            }

            return msg;
        }
    }
}