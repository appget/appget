using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.Extentions;

namespace AppGet.Manifests
{
    public class WindowsVersion
    {
        public string Name { get; private set; }
        public Version Version { get; private set; }
        public int ServicePack { get; private set; }

        private WindowsVersion(string name, Version version, int servicePack = 0)
        {
            Name = name;
            Version = version;
            ServicePack = servicePack;
        }

        private static readonly Regex ParsingRegex = new Regex(@"^(?:Windows\s)?(?<name>.+?)(?:\s(?:SP|Service Pack\s)(?<servicepack>\d)|$)",
                                                               RegexOptions.Compiled);


        public override string ToString()
        {
            var result = String.Format("Windows {0}", Name);

            if (ServicePack > 0)
            {
                result += String.Format(" SP{0}", ServicePack);
            }

            return result;
        }


        public static WindowsVersion FromOperatingSystem(OperatingSystem osVersion)
        {
            var servicePack = 0;
            var version = new Version(osVersion.Version.Major, osVersion.Version.Minor);

            if (!string.IsNullOrEmpty(osVersion.ServicePack))
            {
                var lastPart = osVersion.ServicePack.Split(' ').Last();

                servicePack = Int32.Parse(lastPart);
            }

            return All.Single(c => c.Version == version && c.ServicePack == servicePack);
        }

        public static WindowsVersion Parse(string input)
        {
            var match = ParsingRegex.Match(input);

            if (!match.Success)
            {
                throw new WindowsVersionParseException(input);
            }

            var name = match.Groups["name"].Value;
            var spString = match.Groups["servicepack"].Value;

            if (spString.IsNullOrWhiteSpace())
            {
                spString = "0";
            }

            var sp = Convert.ToInt32(spString);

            var os = All.Where(o => o.Name == name &&
                                       o.ServicePack == sp).ToList();

            if (os.Count == 0)
            {
                throw new WindowsVersionParseException(input);
            }

            return os.FirstOrDefault();
        }

        public static bool operator <(WindowsVersion a, WindowsVersion b)
        {
            if ((object)a == null || (object)b == null) return false;

            if (a.Version != b.Version)
            {
                return a.Version < b.Version;
            }

            return a.ServicePack < b.ServicePack;
        }

        public static bool operator >(WindowsVersion a, WindowsVersion b)
        {
            if ((object)a == null || (object)b == null) return false;

            if (a.Version != b.Version)
            {
                return a.Version > b.Version;
            }

            return a.ServicePack > b.ServicePack;
        }

        public static bool operator <=(WindowsVersion a, WindowsVersion b)
        {
            return a == b || a < b;
        }

        public static bool operator >=(WindowsVersion a, WindowsVersion b)
        {
            return a == b || a > b;
        }


        public static bool operator ==(WindowsVersion a, WindowsVersion b)
        {
            return (object)a == (object)b;
        }

        public static bool operator !=(WindowsVersion a, WindowsVersion b)
        {
            return !(a == b);
        }


        //XP/2003
        public static WindowsVersion Xp { get { return new WindowsVersion("XP", new Version(5, 1)); } }
        public static WindowsVersion XpSp1 { get { return new WindowsVersion("XP", new Version(5, 1), 1); } }
        public static WindowsVersion XpSp2 { get { return new WindowsVersion("XP", new Version(5, 1), 2); } }
        public static WindowsVersion XpSp3 { get { return new WindowsVersion("XP", new Version(5, 1), 3); } }

        //TODO: What service packs are available?
        public static WindowsVersion Xp64 { get { return new WindowsVersion("XP 64-bit Edition", new Version(5, 2)); } }
        //public static WindowsVersion Server2003 { get { return new WindowsVersion("Server 2003", new Version(5, 2)); } }
        //public static WindowsVersion Server2003Sp1 { get { return new WindowsVersion("Server 2003", new Version(5, 2), 1); } }
        //public static WindowsVersion Server2003Sp2 { get { return new WindowsVersion("Server 2003", new Version(5, 2), 2); } }
        //public static WindowsVersion Server2003R2 { get { return new WindowsVersion("Server 2003", new Version(5, 2)); } }


        //Vista/2008
        public static WindowsVersion Vista { get { return new WindowsVersion("Vista", new Version(6, 0)); } }
        public static WindowsVersion VistaSp1 { get { return new WindowsVersion("Vista", new Version(6, 0), 1); } }
        public static WindowsVersion VistaSp2 { get { return new WindowsVersion("Vista", new Version(6, 0), 2); } }
        //public static WindowsVersion Server2008 { get { return new WindowsVersion("Server 2008", new Version(6, 0)); } }


        //7/2008 R2
        public static WindowsVersion Seven { get { return new WindowsVersion("7", new Version(6, 1)); } }
        public static WindowsVersion SevenSp1 { get { return new WindowsVersion("7", new Version(6, 1), 1); } }
        //public static WindowsVersion Server2008R2 { get { return new WindowsVersion("Server 2008 R2", new Version(6, 1)); } }


        //8/2012
        public static WindowsVersion Eight { get { return new WindowsVersion("8", new Version(6, 2)); } }
        //public static WindowsVersion Server2012 { get { return new WindowsVersion("Server 2012", new Version(6, 2)); } }


        //8.1/2012 R2
        public static WindowsVersion EightOne { get { return new WindowsVersion("8.1", new Version(6, 3)); } }
        //public static WindowsVersion Server2012R2 { get { return new WindowsVersion("Server 2012 R2", new Version(6, 3)); } }

        private static List<WindowsVersion> All
        {
            get
            {
                return new List<WindowsVersion>
                       {
                           Xp,
                           XpSp1,
                           XpSp2,
                           XpSp3,
                           Xp64,
                           //Server2003,
                           //Server2003Sp1,
                           //Server2003Sp2,
                           //Server2003R2,
                           Vista,
                           VistaSp1,
                           VistaSp2,
                           //Server2008,
                           Seven,
                           SevenSp1,
                           //Server2008R2,
                           Eight,
                           //Server2012,
                           EightOne,
                           //Server2012R2
                       };
            }
        }
    }
}
