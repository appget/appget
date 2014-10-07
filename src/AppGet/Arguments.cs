using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AppGet
{
    public class Arguments
    {
        private static readonly Regex CommandNameRegex = new Regex(@"^\w+\s", RegexOptions.Compiled);

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

        }

        public string Command { get; private set; }
        public HashSet<string> Flags { get; private set; }
        public Dictionary<string, string> Params { get; private set; }
    }
}