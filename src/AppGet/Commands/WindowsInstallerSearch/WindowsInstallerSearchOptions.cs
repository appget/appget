using CommandLine;

namespace AppGet.Commands.WindowsInstallerSearch
{
    [Verb("windows-installer", HelpText = "????")]
    public class WindowsInstallerSearchOptions : PackageCommandOptions
    {
        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "ID of package to search for", Required = true)]
        public override string PackageId { get; set; }
    }
}