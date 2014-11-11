using System;
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

            return GetUninstallRecords(registryKey).Concat(GetUninstallRecords(registryKey64)).ToList();
        }

        public IEnumerable<UninstallRecord> GetUninstallRecords(string path)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(path, false))
            {
                foreach (var subkeyName in key.GetSubKeyNames())
                {
                    var subKey = key.OpenSubKey(subkeyName);

                    if (subKey != null && subKey.GetValue("DisplayName") != null)
                    {
                        var record = new UninstallRecord
                        {
                            Id = subkeyName,
                            Name = subKey.GetValue("DisplayName").ToString(),
                            InstallLocation = subKey.GetValue("InstallLocation") != null ? subKey.GetValue("InstallLocation").ToString() : null,
                            UninstallCommand = subKey.GetValue("UninstallString") != null ? subKey.GetValue("UninstallString").ToString() : null,
                        };

                        if (subKey.GetValue("QuietUninstallString") != null)
                        {
                            record.UninstallCommand = subKey.GetValue("QuietUninstallString").ToString();
                            record.InstallMethod = InstallMethodType.Inno;
                        }

                        /*if (record.UninstallCommand != null && record.UninstallCommand.Contains("Oarpmany.exe"))
                        {
                            continue;
                        }*/

                        if (record.UninstallCommand != null && record.UninstallCommand.Contains("MsiExec.exe"))
                        {
                            record.InstallMethod = InstallMethodType.MSI;
                        }

                        yield return record;
                    }

                }
            }
        }
    }
}