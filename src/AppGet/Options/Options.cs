using System;
using System.Collections.Generic;
using AppGet.Commands.Install;
using AppGet.Commands.List;
using AppGet.Commands.ShowFlightPlan;
using CommandLine;
using CommandLine.Text;

namespace AppGet.Options
{
    public class RootOptions
    {
        [Option('v', null, HelpText = "Display more detailed execution information about")]
        public bool Verbose { get; set; }

        [VerbOption("showflightplan", HelpText = "Display the FlightPlan for a specific package")]
        public ShowFlightPlanOptions ShowFlightPlan { get; set; }

        [VerbOption("install", HelpText = "Download and install a package")]
        public InstallOptions Install { get; set; }

        [VerbOption("list", HelpText = "List installed packages")]
        public ListOptions List { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = -1)]
        public IList<string> UnknownArgs { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("AppGet", "1.0"),
                Copyright = new CopyrightInfo("AppGet", DateTime.Now.Year),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = false
            };
            help.AddPreOptionsLine("Apache License Version 2.0");
            help.AddOptions(this);

            return help;
        }
    }
}