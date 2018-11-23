using CommandLine;

namespace AppGet.Commands.AddRepo
{
    [Verb("add-repo", HelpText = "Add a remote repository")]
    public class AddRepoOptions : AppGetOption
    {
        [Option(HelpText = "local name for the repository")]
        public string Name { get; set; }

        [Value(1, HelpText = "Connection string for remote repository")]
        public string ConnectionString { get; set; }
    }
}