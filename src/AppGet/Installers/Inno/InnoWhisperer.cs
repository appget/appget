using System;
using AppGet.Commands.Uninstall;
using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Inno
{
    public class InnoWhisperer : InstallerWhispererBase
    {
        public InnoWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.Inno;
        }

        //Command line args: http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline
        protected override string InteractiveArgs => "/SILENT /SUPPRESSMSGBOXES /NORESTART /RESTARTEXITCODE=3010";
        protected override string PassiveArgs => "/SILENT /SUPPRESSMSGBOXES /NORESTART /RESTARTEXITCODE=3010";
        protected override string SilentArgs => "/SILENT /SUPPRESSMSGBOXES /NORESTART /RESTARTEXITCODE=3010";

        protected override string GetLoggingArgs(string path)
        {
            return $"/LOG=\"{path}\"";
        }
    }
}
