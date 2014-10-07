using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
}