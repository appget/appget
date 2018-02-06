using AppGet.Options;
using CommandLine;

namespace AppGet.Commands.Uninstall
{
    [Verb("uninstall", HelpText = "Uninstall a package")]
    public class UninstallOptions : PackageCommandOptions
    {
        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "ID of package to uninstall", Required = true)]
        public override string PackageId { get; set; }

        public UninstallOptions(string packageId)
        {
            PackageId = packageId;
        }
    }
}