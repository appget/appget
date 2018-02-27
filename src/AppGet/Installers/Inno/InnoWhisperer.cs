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

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Inno;
        protected override bool HasLogs => false;

        public override void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        //Command line args: http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline
        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "/SILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /FORCECLOSEAPPLICATIONS /RESTARTEXITCODE=3010";
        protected override string SilentArgs => "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /FORCECLOSEAPPLICATIONS /RESTARTEXITCODE=3010";

        protected override string GetLoggingArgs(string path)
        {
            return $"/LOG=\"{path}\"";
        }
    }
}
