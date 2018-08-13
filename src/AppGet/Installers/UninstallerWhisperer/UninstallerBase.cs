using System;
using System.Collections.Generic;
using System.IO;
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
        protected Dictionary<string, string> UninstallKeys { get; private set; }
        protected UninstallData UninstallData { get; private set; }

        protected UninstallerBase()
        {
            ExitCodes = new Dictionary<int, ExistReason>();
        }


        protected void EnsureUnInstallerInit()
        {
            if (UninstallKeys == null)
            {
                throw new InvalidOperationException("Uninstaller hasn't been initialized.");
            }
        }

        public void InitUninstaller(Dictionary<string, string> keys, UninstallData uninstallData)
        {
            if (UninstallData != null || UninstallKeys != null)
            {
                throw new InvalidOperationException("Can\'t reuse uninstaller.");
            }

            UninstallKeys = keys;
            UninstallData = uninstallData;
        }


        public virtual string GetProcessPath()
        {
            if (UninstallKeys.TryGetValue("UninstallString", out var uninstallString))
            {
                uninstallString = uninstallString.Trim('"');
                var index = uninstallString.IndexOf(@"""");
                return Path.GetFullPath(uninstallString.Substring(0, index)).Trim('"');
            }

            return null;
        }

    }
}