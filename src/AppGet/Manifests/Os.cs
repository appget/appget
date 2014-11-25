using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.Extentions;

namespace AppGet.Manifests
{
    public class Os
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public int ServicePack { get; set; }


        public Os(string name, Version version, int servicePack = 0)
        {
            Name = name;
            Version = version;
            ServicePack = servicePack;
        }

        private static readonly Regex ParsingRegex = new Regex(@"^(?:Windows\s)?(?<name>.+?)(?:\s(?:SP|Service Pack\s)(?<servicepack>\d)|$)", 
                                                               RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public override string ToString()
        {
            var result = String.Format("Windows {0}", Name);

            if (ServicePack > 0)
            {
                result += String.Format(" SP{0}", ServicePack);
            }

            return result;
        }

        public static Os Parse(string input)
        {
            var match = ParsingRegex.Match(input);

            if (!match.Success)
            {
                return Os.Unknown;
            }

            var name = match.Groups["name"].Value;
            var spString = match.Groups["servicepack"].Value;

            if (spString.IsNullOrWhiteSpace())
            {
                spString = "0";
            }

            var sp = Convert.ToInt32(spString);

            var os = Os.All.Where(o => o.Name.ToLowerInvariant() == name.ToLowerInvariant() &&
                                       o.ServicePack == sp).ToList();

            if (os.Count == 0)
            {
                return Os.Unknown;
            }

            return os.FirstOrDefault();
        }

        public static Os Unknown { get { return new Os("Unknown", new Version(0, 0)); } }

        //XP/2003
        public static Os Xp { get { return new Os("XP", new Version(5, 1)); } }
        public static Os XpSp1 { get { return new Os("XP", new Version(5, 1), 1); } }
        public static Os XpSp2 { get { return new Os("XP", new Version(5, 1), 2); } }
        public static Os XpSp3 { get { return new Os("XP", new Version(5, 1), 3); } }

        //TODO: What service packs are available?
        public static Os Xp64 { get { return new Os("XP 64-bit Edition", new Version(5, 2)); } }
        public static Os Server2003 { get { return new Os("Server 2003", new Version(5, 2)); } }
        public static Os Server2003Sp1 { get { return new Os("Server 2003", new Version(5, 2), 1); } }
        public static Os Server2003Sp2 { get { return new Os("Server 2003", new Version(5, 2), 2); } }
        public static Os Server2003R2 { get { return new Os("Server 2003", new Version(5, 2)); } }
        

        //Vista/2008
        public static Os Vista { get { return new Os("Vista", new Version(6, 0)); } }
        public static Os VistaSp1 { get { return new Os("Vista", new Version(6, 0), 1); } }
        public static Os VistaSp2 { get { return new Os("Vista", new Version(6, 0), 2); } }
        public static Os Server2008 { get { return new Os("Server 2008", new Version(6, 0)); } }


        //7/2008 R2
        public static Os Seven { get { return new Os("Server", new Version(6, 1)); } }
        public static Os SevenSp1 { get { return new Os("Server", new Version(6, 1), 1); } }
        public static Os Server2008R2 { get { return new Os("Server 2008 R2", new Version(6, 1)); } }


        //8/2012
        public static Os Eight { get { return new Os("8", new Version(6, 2)); } }
        public static Os Server2012 { get { return new Os("Server 2012", new Version(6, 2)); } }
        

        //8.1/2012 R2
        public static Os EightOne { get { return new Os("8.1", new Version(6, 3)); } }
        public static Os Server2012R2 { get { return new Os("Server 2012 R2", new Version(6, 3)); } }

        public static List<Os> All
        {
            get
            {
                return new List<Os>
                       {
                           Xp,
                           XpSp1,
                           XpSp2,
                           XpSp3,
                           Xp64,
                           Server2003,
                           Server2003Sp1,
                           Server2003Sp2,
                           Server2003R2,
                           Vista,
                           VistaSp1,
                           VistaSp2,
                           Server2008,
                           Seven,
                           SevenSp1,
                           Server2008R2,
                           Eight,
                           Server2012,
                           EightOne,
                           Server2012R2
                       };
            }
        }
    }

    public enum OsType
    {
        Desktop = 1,
        Server = 2
    }
}
