using AppGet.Commands.Install;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public class InitializingViewModel : Screen
    {
        private readonly InstallOptions _options;

        public InitializingViewModel(InstallOptions options)
        {
            _options = options;
        }

        public string Message => $"Initializing Installation for {_options.Package}";
    }
}