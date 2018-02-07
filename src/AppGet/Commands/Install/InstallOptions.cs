using AppGet.Options;
using CommandLine;

namespace AppGet.Commands.Install
{
    [Verb("install", HelpText = "Install a package")]
    public class InstallOptions : PackageCommandOptions
    {
        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "ID of package to install", Required = true)]
        public override string PackageId { get; set; }

        [Option('f', "force", HelpText = "Force the operation")]
        public bool Force { get; set; }

        [Option('i', "interactive", HelpText = "Start the installer in interactive mode. This allows the user to step through the installer manually. (Doesn't apply to Zip installers)")]
        public bool Interactive { get; set; }

        [Option('s', "silent", HelpText = "Attempt to start the installer completely silently without showing any user dialogs")]
        public bool Silent { get; set; }
    }
}