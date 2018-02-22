using System;
using System.Diagnostics;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Msi
{
    public class MsiWhisperer : InstallerWhispererBase
    {
        public MsiWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.MSI;

        protected override Process StartProcess(string installerLocation, string args)
        {
            return base.StartProcess("msiexec", $"/i \"{installerLocation}\" {args}");
        }

        protected override string InteractiveArgs => "/qf";
        protected override string PassiveArgs => "/qb /norestart";
        protected override string SilentArgs => "/qn /norestart";

        protected override string GetLoggingArgs(string path)
        {
            return $"/L* \"{path}\"";
        }
    }
}
