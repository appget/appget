using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.Infrastructure.Events;
using AppGet.Installers.Events;

namespace AppGet.Gui.Views.Installation
{
    public class InstallCommandViewModel : CommandViewModel<InstallOptions>
    {
        private readonly ICommandExecutor _executor;
        private readonly ITinyMessengerHub _hub;
        private readonly InitializingViewModel _initializingViewModel;
        private readonly DownloadProgressViewModel _installProgressViewModel;
        private readonly InstallingViewModel _installingViewModel;
        private readonly InstallationSuccessfulViewModel _installationSuccessfulViewModel;
        private TinyMessageSubscriptionToken _progressToken;

        public InstallCommandViewModel(ICommandExecutor executor, ITinyMessengerHub hub,
            InitializingViewModel initializingViewModel,
            DownloadProgressViewModel installProgressViewModel,
            InstallingViewModel installingViewModel,
            InstallationSuccessfulViewModel installationSuccessfulViewModel)
        {
            _executor = executor;
            _hub = hub;
            _initializingViewModel = initializingViewModel;
            _installProgressViewModel = installProgressViewModel;
            _installingViewModel = installingViewModel;
            _installationSuccessfulViewModel = installationSuccessfulViewModel;
        }

        protected override void OnActivate()
        {
            ActivateItem(_initializingViewModel);

            Task.Run(() =>
            {
                _executor.ExecuteCommand(Options);
            });


            _hub.Subscribe<FileTransferStartedEvent>(e =>
            {
                ActivateItem(_installProgressViewModel);
            });

            _hub.Subscribe<ExecutingInstallerEvent>(e =>
            {
                ActivateItem(_installingViewModel);
            });

            _hub.Subscribe<InstallationSuccessfulEvent>(e =>
            {
                ActivateItem(_installationSuccessfulViewModel);
            });
        }
    }
}