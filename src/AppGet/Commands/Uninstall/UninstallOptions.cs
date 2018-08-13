using AppGet.Installers;
using CommandLine;

namespace AppGet.Commands.Uninstall
{
    [Verb("uninstall", HelpText = "Uninstall a package")]
    public class UninstallOptions : PackageCommandOptions
    {
        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "package to uninstall", Required = true)]
        public override string Package { get; set; }

        private bool _interactive;
        private bool _silent;

        [Option('f', "force", HelpText = "Force the operation")]
        public bool Force { get; set; }

        [Option('i', "interactive", HelpText =
            "Start the uninstaller in interactive mode. This allows the user to step through the uninstaller manually.")]
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

        [Option('s', "silent", HelpText = "Attempt to start the uninstaller completely silently without showing any dialogs")]
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

        public InstallInteractivityLevels InteractivityLevel
        {
            get
            {
                if (Interactive) return InstallInteractivityLevels.Interactive;
                if (Silent) return InstallInteractivityLevels.Silent;
                return InstallInteractivityLevels.Passive;
            }

            set
            {
                switch (value)
                {
                    case InstallInteractivityLevels.Interactive:
                        {
                            Interactive = true;
                            break;
                        }
                    case InstallInteractivityLevels.Passive:
                        {
                            Passive = true;
                            break;
                        }
                    case InstallInteractivityLevels.Silent:
                        {
                            Silent = true;
                            break;
                        }
                }
            }
        }
    }
}