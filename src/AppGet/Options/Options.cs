using System;
using System.Collections.Generic;
using System.Text;
using AppGet.Commands.Install;
using AppGet.Commands.List;
using AppGet.Commands.Search;
using AppGet.Commands.ShowFlightPlan;
using AppGet.Commands.Uninstall;
using CommandLine;
using CommandLine.Text;

namespace AppGet.Options
{
    public class RootOptions
    {
        [VerbOption("install", HelpText = "Download and install a package")]
        public InstallOptions Install { get; set; }

        [VerbOption("uninstall", HelpText = "Uninstall a previously installed package")]
        public UninstallOptions Uninstall { get; set; }

        [VerbOption("list", HelpText = "List installed packages")]
        public ListOptions List { get; set; }

        [VerbOption("search", HelpText = "Search package repository")]
        public SearchOptions Search { get; set; }

        [VerbOption("view", HelpText = "Display the FlightPlan for a specific package")]
        public ViewFlightPlanOptions ViewFlightPlan { get; set; }

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