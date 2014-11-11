using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppGet.FlightPlans;
using Microsoft.Win32;

namespace AppGet.InstalledPackages
{
    public class WindowsInstallerInventoryManager
    {
        public List<UninstallRecord> GetInstalledApplication()
        {
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string registryKey64 = @"SOFTWARE\WOW6432node\Microsoft\Windows\CurrentVersion\Uninstall";

            return GetUninstallRecordsForLocalMachine(registryKey)
                    .Concat(GetUninstallRecordsForLocalMachine(registryKey64))
                    .Concat(GetUninstallRecordsForCurrentUser(registryKey))
                    .Concat(GetUninstallRecordsForCurrentUser(registryKey64))
                    .ToList();
        }

        private List<UninstallRecord> GetUninstallRecordsForLocalMachine(string path)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(path, false))
            {
                if (key != null)
                {
                    return GetUninstallRecords(key).ToList();
                }
            }

            return new List<UninstallRecord>(0);
        }

        private List<UninstallRecord> GetUninstallRecordsForCurrentUser(string path)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(path, false))
            {
                if (key != null)
                {
                    return GetUninstallRecords(key).ToList();
                }
            }

            return new List<UninstallRecord>(0);
        }

        private IEnumerable<UninstallRecord> GetUninstallRecords(RegistryKey key)
        {
            foreach (var subKeyName in key.GetSubKeyNames())
            {
                var subKey = key.OpenSubKey(subKeyName);

                if (subKey != null && subKey.GetValue("DisplayName") != null)
                {
                    var record = GetUninstallRecord(subKey, subKeyName);

                    yield return record;
                }
            }
        }

        private UninstallRecord GetUninstallRecord(RegistryKey key, string name)
        {
            var record = new UninstallRecord
            {
                Id = name,
                Name = key.GetValue("DisplayName").ToString(),
                InstallLocation = key.GetValue("InstallLocation") != null ? key.GetValue("InstallLocation").ToString() : null,
                UninstallCommand = key.GetValue("UninstallString") != null ? key.GetValue("UninstallString").ToString() : null,
            };

            if (key.GetValue("QuietUninstallString") != null)
            {
                record.UninstallCommand = key.GetValue("QuietUninstallString").ToString();
                record.InstallMethod = InstallMethodType.Inno;
            }

            /*if (record.UninstallCommand != null && record.UninstallCommand.Contains("Oarpmany.exe"))
            {
                continue;
            }*/

            if (record.UninstallCommand != null &&
                record.UninstallCommand.Contains("rundll32.exe dfshim.dll,ShArpMaintain"))
            {
                record.InstallMethod = InstallMethodType.ClickOnce;
            }

            if (record.UninstallCommand != null && record.UninstallCommand.Contains("MsiExec.exe"))
            {
                record.InstallMethod = InstallMethodType.MSI;
            }

            return record;
        }
    }
}