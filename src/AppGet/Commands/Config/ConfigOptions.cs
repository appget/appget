using CommandLine;

namespace AppGet.Commands.Config
{
    [Verb("config", HelpText = "View and Update config options", Hidden = true)]
    public class ConfigOptions : AppGetOption
    {
        [Value(1, HelpText = "Supported actions are: list, set")]
        public string Action { get; set; }

        [Value(1, HelpText = "Config Key", Required = false)]
        public string Key { get; set; }

        [Option(HelpText = "Config Value", Required = false)]
        public string Value { get; set; }
    }
}