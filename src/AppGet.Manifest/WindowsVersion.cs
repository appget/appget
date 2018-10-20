using System;
using System.Collections.Generic;

namespace AppGet.Manifest
{
    public static class WindowsVersion
    {
        public static readonly IEnumerable<Version> KnownVersions = new[]
        {
            new Version(0, 0),
            new Version(6, 1),
            new Version(6, 2),
            new Version(6, 3),
            new Version(10, 0)
        };

        public static string ToServerName(Version version)
        {
            switch (version.ToString(2))
            {
                case "0.0":
                {
                    return "Any";
                }
                case "6.1":
                {
                    return "Windows Server 2008 R2";
                }
                case "6.2":
                {
                    return "Windows Server 2012";
                }
                case "6.3":
                {
                    return "Windows Server 2012 R2";
                }
                case "10.0":
                {
                    return "Windows Server 2016";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(version), $"Unknown or unsupported Windows Version. {version}");
                }
            }
        }

        public static string ToDesktopName(Version version)
        {
            switch (version.ToString(2))
            {
                case "0.0":
                {
                    return "Any";
                }
                case "6.1":
                {
                    return "Windows 7";
                }
                case "6.2":
                {
                    return "Windows 8";
                }
                case "6.3":
                {
                    return "Windows 8.1";
                }
                case "10.0":
                {
                    return "Windows 10";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(version), $"Unknown or unsupported Windows Version. {version}");
                }
            }
        }
    }
}