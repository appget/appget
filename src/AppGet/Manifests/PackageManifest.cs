﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AppGet.Manifests
{
    [DebuggerDisplay("{" + nameof(Id) + "} " + "{" + nameof(Version) + "}")]
    public class PackageManifest
    {
        public const string LATEST_TAG = "latest";
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string Home { get; set; }
        public string Repo { get; set; }
        public string License { get; set; }

        public string[] Exe { get; set; }

        public InstallMethodTypes InstallMethod { get; set; }
        public InstallArgs Args { get; set; }

        public List<Installer> Installers { get; set; }

        public PackageManifest()
        {
            InstallMethod = InstallMethodTypes.Custom;
        }
    }

    public class InstallArgs
    {
        public string Passive { get; set; }
        public string Silent { get; set; }
        public string Interactive { get; set; }
        public string Log { get; set; }
    }

    public class Installer
    {
        public string Location { get; set; }
        public string ZipSubDir { get; set; }

        public string Sha1 { get; set; }
        public string Sha256 { get; set; }
        public string Md5 { get; set; }

        public ArchitectureTypes Architecture { get; set; }

        public Version MinWindowsVersion { get; set; }

        public List<string> ProductIds { get; set; }

        public FileVerificationInfo GetFileHash()
        {
            if (!string.IsNullOrEmpty(Sha256))
            {
                return new FileVerificationInfo
                {
                    HashType = HashTypes.Sha256,
                    HashValue = Sha256
                };
            }

            if (!string.IsNullOrEmpty(Sha1))
            {
                return new FileVerificationInfo
                {
                    HashType = HashTypes.Sha1,
                    HashValue = Sha1
                };
            }

            if (!string.IsNullOrEmpty(Md5))
            {
                return new FileVerificationInfo
                {
                    HashType = HashTypes.Md5,
                    HashValue = Md5
                };
            }

            return new FileVerificationInfo();
        }
    }
}