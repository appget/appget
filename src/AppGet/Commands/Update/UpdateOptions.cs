using CommandLine;

namespace AppGet.Commands.Update
{
    [Verb("update", HelpText = "Update one or more of installed applications")]
    public class UpdateOptions : PackageCommandOptions
    {
        private bool _interactive;
        private bool _silent;

        [Value(0, MetaName = PACKAGE_META_NAME, HelpText = "Package to update", Required = true)]
        public override string Package { get; set; }

        [Option('i', "interactive", HelpText = "Start the installer in interactive mode. This allows the user to step through the installer manually.")]
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