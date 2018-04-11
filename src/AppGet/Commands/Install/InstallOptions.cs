using CommandLine;

namespace AppGet.Commands.Install
{
    [Verb("install", HelpText = "Install a package")]
    public class InstallOptions : PackageCommandOptions
    {
        private bool _interactive;
        private bool _silent;

        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "Package to install", Required = true)]
        public override string Package { get; set; }

        [Option('f', "force", HelpText = "Force the operation")]
        public bool Force { get; set; }

        [Option('i', "interactive", HelpText =
            "Start the installer in interactive mode. This allows the user to step through the installer manually. (Doesn't apply to Zip installers)")]
        public bool Interactive
        {
            get => _interactive;
            set
            {
                if (value)
                {
                    Silent = false;
                }

                _interactive = value;
            }
        }

        [Option('s', "silent", HelpText = "Attempt to start the installer completely silently without showing any user dialogs")]
        public bool Silent
        {
            get => _silent;
            set
            {
                if (value)
                {
                    Interactive = false;
                }

                _silent = value;
            }
        }

        public bool Passive
        {
            get => !Silent & !Interactive;
            set
            {
                Silent = false;
                Interactive = false;
            }
        }
    }
}