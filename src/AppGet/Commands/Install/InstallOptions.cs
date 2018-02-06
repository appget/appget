using AppGet.Options;
using CommandLine;

namespace AppGet.Commands.Install
{
    [Verb("install", HelpText = "Install a package")]
    public class InstallOptions : PackageCommandOptions
    {
        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "ID of package to install", Required = true)]
        public override string PackageId { get; set; }

        public InstallOptions(string packageId)
        {
            PackageId = packageId;
        }
    }
}