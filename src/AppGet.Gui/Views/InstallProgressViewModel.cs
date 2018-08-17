using System.Windows;
using AppGet.Commands.Install;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public class InstallProgressViewModel : Screen
    {
        private readonly InstallOptions _installOptions;

        public InstallProgressViewModel(InstallOptions installOptions)
        {
            _installOptions = installOptions;
            ProgressMaximum = 75;
            //            Progress = 50;

            ProgressStatus = $"Installing {installOptions.Package}";
        }

        public string ProgressStatus { get; private set; }

        public int Progress { get; private set; }
        public int ProgressMaximum { get; private set; }

        public bool ProgressIndeterminate => Progress == 0;
    }
}