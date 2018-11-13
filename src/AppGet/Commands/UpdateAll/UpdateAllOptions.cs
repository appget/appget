using CommandLine;

namespace AppGet.Commands.UpdateAll
{
    [Verb("update-all", HelpText = "Batch update all outdated applications. Running as Administrator is highly recommended.")]
    public class UpdateAllOptions : AppGetOption, IVariableInteractivityCommand
    {
        private bool _interactive;
        private bool _silent;

        [Option('i', "interactive", HelpText = "Start the installers in interactive mode. This allows the user to step through the installer manually.")]
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

        [Option('s', "silent", HelpText = "Attempt to start the installers completely silently without showing any user dialogs")]
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