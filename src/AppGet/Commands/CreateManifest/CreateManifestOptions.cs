using CommandLine;

namespace AppGet.Commands.CreateManifest
{
    [Verb("create", HelpText = "Create a package manifest")]
    public class CreateManifestOptions : AppGetOption
    {
        [Value(0, MetaName = "URL", HelpText = "Installer download URL", Required = true)]
        public string DownloadUrl { get; set; }
    }
}