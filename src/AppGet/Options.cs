using System.Diagnostics;
using System.Security.AccessControl;
using System.Text;
using AppGet.Commands;
using CommandLine;

namespace AppGet
{

    public abstract class VerbOptions
    {
        public Options Common { get; set; }
    }

    public class ShowFlightPlanOptions : VerbOptions
    {

    }

    public class Options
    {
        private static VerbOptions verbOptions;

        public static VerbOptions Parse(string[] args)
        {
            var options = new Options(args);

            Parser.Default.ParseArguments(args, options, (verb, subOptions) =>
            {

                if (subOptions == null)
                {
                    throw new UnknownCommandException(verb);
                }

                verbOptions = ((VerbOptions)subOptions);
                verbOptions.Common = options;

            });

            return verbOptions;
        }

        private Options(string[] args)
        {
            Parser.Default.ParseArguments(args, this, (verb, subOptions) => { });
        }

        [Option('v', null, HelpText = "Display more detailed execution information about")]
        public bool Verbose { get; set; }

        [VerbOption("showflightplan", HelpText = "Disaply the FlightPlan for a specific package")]
        public ShowFlightPlanOptions ShowFlightPlan { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("Quickstart Application 1.0");
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }
}