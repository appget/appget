using CommandLine;

namespace AppGet.Commands.Config
{
    [Verb("config", HelpText = "View and Update config options", Hidden = true)]
    public class ConfigOptions : AppGetOption
    {
        [Value(100, MetaName = "abc", HelpText = "Supported actions are: list, set", Required = true)]
        public string Action { get; set; }

        [Option('k', "key", HelpText = "Key to set", Required = false)]
        public string Key { get; set; }

        [Option(shortName: 'a', "value", HelpText = "New value", Required = false)]
        public string Value { get; set; }
    }
}