using System.Text;
using AppGet.Commands;
using CommandLine;

namespace AppGet
{

    public abstract class CommandOptions
    {
        public string CommandName { get; set; }
        public Options Common { get; set; }
    }

    public class ShowFlightPlanOptions : CommandOptions
    {

    }

    public class Options
    {

        public static CommandOptions Parse(string[] args)
        {
            var options = new Options(args);

            CommandOptions commandOptions = null;

            Parser.Default.ParseArguments(args, options, (verb, subOptions) =>
            {

                if (subOptions == null)
                {
                    throw new UnknownCommandException(verb);
                }

                commandOptions = ((CommandOptions)subOptions);
                commandOptions.Common = options;
                commandOptions.CommandName = verb;

            });

            return commandOptions;
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