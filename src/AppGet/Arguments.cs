using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CommandLine;

namespace AppGet
{
    public class Arguments
    {
        private static readonly Regex CommandNameRegex = new Regex(@"^\w+\s", RegexOptions.Compiled);
        private static readonly Regex ArgumentsRegex = new Regex(@"(?<flag>(?<=[-/])[a-z]+?\b)(?:[:=](?<value>""[^""]+""|[^""]*?(?=\s|$)))?",
                                                                 RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Arguments(string arguments)
        {
            Flags = new HashSet<string>();
            Params = new Dictionary<string, string>();

            arguments = arguments.Trim();

            var commandName = CommandNameRegex.Match(arguments);

            if (commandName.Success)
            {
                Command = commandName.Value.Trim().ToLower();
            }

            var argumentMatches = ArgumentsRegex.Matches(arguments);

            foreach (Match argumentMatch in argumentMatches)
            {
                var flag = argumentMatch.Groups["flag"].Value;
                var value = argumentMatch.Groups["value"].Value;

                if (String.IsNullOrEmpty(value))
                {
                    Flags.Add(flag);
                }

                else
                {
                    Params.Add(flag, value.Trim('"', ' '));
                }
            }
        }

        public string Command { get; private set; }
        public HashSet<string> Flags { get; private set; }
        public Dictionary<string, string> Params { get; private set; }
    }

    public class Options
    {

        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }

        [Option("length", DefaultValue = -1, HelpText = "The maximum number of bytes to process.")]
        public int MaximumLength { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

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