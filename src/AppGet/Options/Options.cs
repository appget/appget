using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        bool Verbose { get; set; }

        [VerbOption("install", HelpText = "Download and install a package")]
        public InstallOptions Install { get; set; }

        [VerbOption("list", HelpText = "List installed packages")]
        public ListOptions List { get; set; }

        [VerbOption("showflightplan", HelpText = "Display the FlightPlan for a specific package")]
        public ShowFlightPlanOptions ShowFlightPlan { get; set; }

        [ValueList(typeof(List<string>), MaximumElements = -1)]
        public IList<string> UnknownArgs { get; set; }

        public string GetUsage()
        {
            var help = new HelpText
            {
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = false
            };
            help.AddOptions(this);


            //return help;

            var verbLines = help.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //var verbLines = options.Where(c => !c.Trim().StartsWith("-"));

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("usage: AppGet <command> [args]");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine("Available commands:");
            stringBuilder.AppendLine();

            foreach (var verbLine in verbLines)
            {
                stringBuilder.AppendLine(verbLine);
                stringBuilder.AppendLine();
            }



            return stringBuilder.ToString();
        }
    }
}