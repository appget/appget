using CommandLine;

namespace AppGet.Commands.ViewManifest
{
    [Verb("view", HelpText = "Display install manifest for a specific package")]
    public class ViewManifestOptions : PackageCommandOptions
    {
        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "package to display", Required = true)]
        public override string Package { get; set; }
    }
}