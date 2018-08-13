using System;
using System.Collections.Generic;
using AppGet.Installers.UninstallerWhisperer;
using AppGet.Manifest;

namespace AppGet.Installers
{
    public abstract class InstallerBase : IInstaller
    {
        public virtual Dictionary<int, ExistReason> ExitCodes { get; }
        public abstract InstallMethodTypes InstallMethod { get; }
        public virtual string InteractiveArgs => "";
        public abstract string PassiveArgs { get; }
        public abstract string SilentArgs { get; }
        public abstract string LogArgs { get; }

        protected string InstallerPath { get; private set; }

        protected InstallerBase()
        {
            ExitCodes = new Dictionary<int, ExistReason>();
        }

        public virtual string GetProcessPath()
        {
            EnsureInstallerInit();
            return InstallerPath;
        }

        protected void EnsureInstallerInit()
        {
            if (InstallerPath == null)
            {
                throw new InvalidOperationException("Installer hasn't been initialized.");
            }
        }

        public void Initialize(PackageManifest packageManifest, string installerPath)
        {
            if (InstallerPath != null)
            {
                throw new InvalidOperationException("Can\'t reuse installer.");
            }

            InstallerPath = installerPath;
        }
    }
}