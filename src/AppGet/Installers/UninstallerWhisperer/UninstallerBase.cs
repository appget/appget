using System;
using System.Collections.Generic;
using AppGet.Manifest;
using AppGet.Update;

namespace AppGet.Installers.UninstallerWhisperer
{
    public interface IInstaller
    {
        Dictionary<int, ExistReason> ExitCodes { get; }
        InstallMethodTypes InstallMethod { get; }
        string InteractiveArgs { get; }
        string PassiveArgs { get; }
        string SilentArgs { get; }
        string LogArgs { get; }
        string GetProcessPath();
    }

    public abstract class UninstallerBase : IInstaller
    {
        public virtual Dictionary<int, ExistReason> ExitCodes { get; }
        public abstract InstallMethodTypes InstallMethod { get; }
        public abstract string InteractiveArgs { get; }
        public abstract string PassiveArgs { get; }
        public abstract string SilentArgs { get; }
        public abstract string LogArgs { get; }
        protected UninstallData UninstallData { get; private set; }

        protected UninstallerBase()
        {
            ExitCodes = new Dictionary<int, ExistReason>();
        }


        protected void EnsureUnInstallerInit()
        {

        }

        public void InitUninstaller(UninstallData uninstallData)
        {
            if (UninstallData != null)
            {
                throw new InvalidOperationException("Can\'t reuse uninstaller.");
            }

            UninstallData = uninstallData;
        }


        public virtual string GetProcessPath()
        {
            return UninstallData.UninstallerExe;
        }

    }
}