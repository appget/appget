using System.Collections.Generic;
using System.Linq;
using AppGet.Extensions;
using AppGet.Manifests;
using AppGet.Serialization;
using Microsoft.Win32;

namespace AppGet.InstalledPackages
{
    public interface IWindowsInstallerInventoryManager
    {
        List<WindowsInstallRecord> GetInstalledApplications();
        IEnumerable<WindowsInstallRecord> GetInstalledApplications(string name);
    }

    public class WindowsInstallerInventoryManager : IWindowsInstallerInventoryManager
    {
        public List<WindowsInstallRecord> GetInstalledApplications()
        {
            const string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            const string registryKey64 = @"SOFTWARE\WOW6432node\Microsoft\Windows\CurrentVersion\Uninstall";

            return GetInstallRecordsForLocalMachine(registryKey)
                    .Concat(GetInstallRecordsForLocalMachine(registryKey64))
                    .Concat(GetInstallRecordsForCurrentUser(registryKey))
                    .Concat(GetInstallRecordsForCurrentUser(registryKey64))
                    .Distinct(JsonComparator<WindowsInstallRecord>.Comparator)
                    .ToList();
        }


        public IEnumerable<WindowsInstallRecord> GetInstalledApplications(string name)
        {
            var packages = GetInstalledApplications();
            var targetName = name.ToAlphaNumeric();
            return packages.Where(c => c.Name.ToAlphaNumeric().Contains(targetName));
        }


        private List<WindowsInstallRecord> GetInstallRecordsForLocalMachine(string path)
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

        private List<WindowsInstallRecord> GetInstallRecordsForCurrentUser(string registeryPath)
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

                    if (record != null)
                    {
                        yield return record;
                    }
                }
            }
        }

        private static WindowsInstallRecord GetRecords(RegistryKey registryKey, string name)
        {
            var names = registryKey.GetValueNames();
            if (names.Contains("SystemComponent"))
            {
                return null;
            }

            string GetValue(string key)
            {
                return registryKey.GetValue(key)?.ToString();
            }

            var record = new WindowsInstallRecord
            {
                RegistryKey = registryKey,
                Id = name,
                Name = GetValue("DisplayName"),
                Version = GetValue("DisplayVersion"),
                InstallDate = GetValue("InstallDate"),
                Publisher = GetValue("Publisher"),
                InstallLocation = GetValue("InstallLocation"),
                UninstallCommand = GetValue("UninstallString"),
                QuietUninstallCommand = GetValue("QuietUninstallString"),
                InstallSource = GetValue("InstallSource"),
                InstallMethod = InstallMethodTypes.Custom

            };

            if (names.Any(c => c.StartsWith("Inno")))
            {
                record.InstallMethod = InstallMethodTypes.Inno;
            }
            else if (record.UninstallCommand != null)
            {
                if (record.UninstallCommand.ToLowerInvariant().Contains("msiexec.exe"))
                {
                    record.InstallMethod = InstallMethodTypes.MSI;
                }
                else if (record.UninstallCommand.Contains(" --uninstall") && record.UninstallCommand.Contains("Update.exe"))
                {
                    record.InstallMethod = InstallMethodTypes.Squirrel;
                }
            }

            return record;
        }
    }
}