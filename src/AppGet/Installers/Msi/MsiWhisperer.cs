using System;
using AppGet.Commands.Install;
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

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.InstallShield;
        }

        protected override string InteractiveArgs => "";
        protected override string UnattendedArgs => "/s /v\"/quiet /norestart /Liwemoar!";
        protected override string SilentArgs => "/s /v\"/quiet /norestart /Liwemoar!";
        protected override string LoggingArgs => "";
    }
}
