using System.Collections.Generic;
using System.Linq;
using AppGet.Extensions;
using AppGet.Manifest;
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
            return GetInstallRecords(RegistryHive.LocalMachine, RegistryView.Registry32)
                .Concat(GetInstallRecords(RegistryHive.LocalMachine, RegistryView.Registry64))
                .Concat(GetInstallRecords(RegistryHive.CurrentUser, RegistryView.Registry32))
                .Concat(GetInstallRecords(RegistryHive.CurrentUser, RegistryView.Registry64))
                .GroupBy(r => r.Id)
                .Select(g => g.First())
                .OrderBy(c => c.Name)
                .ToList();
        }

        public IEnumerable<WindowsInstallRecord> GetInstalledApplications(string name)
        {
            var packages = GetInstalledApplications();
            var targetName = name.ToAlphaNumeric();

            return packages.Where(c => c.Name.ToAlphaNumeric().Contains(targetName));
        }

        private List<WindowsInstallRecord> GetInstallRecords(RegistryHive hive, RegistryView view)
        {
            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            using (var key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false))
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