using System.Collections.Generic;
using System.Linq;
using AppGet.Manifests;
using Microsoft.Win32;

namespace AppGet.InstalledPackages
{
    public interface IWindowsInstallerInventoryManager
    {
        List<WindowsInstallRecord> GetInstalledApplications();
    }

    public class WindowsInstallerInventoryManager : IWindowsInstallerInventoryManager
    {
        public List<WindowsInstallRecord> GetInstalledApplications()
        {
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string registryKey64 = @"SOFTWARE\WOW6432node\Microsoft\Windows\CurrentVersion\Uninstall";

            return GetUninstallRecordsForLocalMachine(registryKey)
                    .Concat(GetUninstallRecordsForLocalMachine(registryKey64))
                    .Concat(GetUninstallRecordsForCurrentUser(registryKey))
                    .Concat(GetUninstallRecordsForCurrentUser(registryKey64))
                    .ToList();
        }

        private List<WindowsInstallRecord> GetUninstallRecordsForLocalMachine(string path)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(path, false))
            {
                if (key != null)
                {
                    return GetRecords(key).ToList();
                }
            }

            return new List<WindowsInstallRecord>(0);
        }

        private List<WindowsInstallRecord> GetUninstallRecordsForCurrentUser(string registeryPath)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(registeryPath, false))
            {
                if (key != null)
                {
                    return GetRecords(key).ToList();
                }
            }

            return new List<WindowsInstallRecord>(0);
        }

        private IEnumerable<WindowsInstallRecord> GetRecords(RegistryKey registryKey)
        {
            foreach (var subKeyName in registryKey.GetSubKeyNames())
            {
                var subKey = registryKey.OpenSubKey(subKeyName);

                if (subKey != null && subKey.GetValue("DisplayName") != null)
                {
                    var record = GetRecords(subKey, subKeyName);

                    yield return record;
                }
            }
        }

        private static WindowsInstallRecord GetRecords(RegistryKey registryKey, string name)
        {

            string GetValue(string key)
            {
                return registryKey.GetValue(key)?.ToString();
            }

            var record = new WindowsInstallRecord
            {
                Id = name,
                Name = GetValue("DisplayName"),
                Version = GetValue("DisplayVersion"),
                InstallDate = GetValue("InstallDate"),
                Publisher = GetValue("Publisher"),
                InstallLocation = GetValue("InstallLocation"),
                UninstallCommand = GetValue("UninstallString"),
                QuietUninstallCommand = GetValue("QuietUninstallString"),
                InstallSource = GetValue("InstallSource")

            };

            if (registryKey.GetValue("QuietUninstallString") != null)
            {
                record.UninstallCommand = registryKey.GetValue("QuietUninstallString").ToString();
                record.InstallMethod = InstallMethodType.Inno;
            }

            /*if (record.UninstallCommand != null && record.UninstallCommand.Contains("Oarpmany.exe"))
            {
                continue;
            }*/

            if (record.UninstallCommand != null &&
                record.UninstallCommand.ToLowerInvariant().Contains("rundll32.exe dfshim.dll,sharpmaintain"))
            {
                record.InstallMethod = InstallMethodType.ClickOnce;
            }

            if (record.UninstallCommand != null && record.UninstallCommand.ToLowerInvariant().Contains("msiexec.exe"))
            {
                record.InstallMethod = InstallMethodType.MSI;
            }

            return record;
        }
    }
}