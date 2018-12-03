using CommandLine;

namespace AppGet.Commands.AddRepo
{
    [Verb("repo", HelpText = "Add a remote repository", Hidden = true)]
    public class AddRepoOptions : AppGetOption
    {
        [Value(1, HelpText = "Supported actions are: list, add, remove")]
        public string Action { get; set; }

        [Value(1, HelpText = "Connection string for remote repository")]
        public string ConnectionString { get; set; }

        [Option(HelpText = "local name for the repository")]
        public string Name { get; set; }
    }
}