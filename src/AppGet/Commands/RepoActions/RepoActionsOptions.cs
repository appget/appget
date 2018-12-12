using CommandLine;

namespace AppGet.Commands.RepoActions
{
    [Verb("repo", HelpText = "Manage remote repositories remote repository", Hidden = true)]
    public class RepoActionsOptions : AppGetOption
    {
        [Value(1, HelpText = "Supported actions are: list, add, remove", Required = true)]
        public string Action { get; set; }

        [Value(2, HelpText = "Connection string for remote repository")]
        public string ConnectionString { get; set; }

        [Option(HelpText = "Repository name")]
        public string Name { get; set; }

        [Option(HelpText = "Repository ID")]
        public string Id { get; set; }
    }
}