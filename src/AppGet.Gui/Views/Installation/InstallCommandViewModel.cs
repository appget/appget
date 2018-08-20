using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.Infrastructure.Events;

namespace AppGet.Gui.Views.Installation
{
    public class InstallCommandViewModel : CommandViewModel<InstallOptions>
    {
        private readonly ICommandExecutor _executor;
        private readonly ITinyMessengerHub _hub;
        private readonly InitializingViewModel _initializingViewModel;
        private readonly InstallProgressViewModel _installProgressViewModel;
        private TinyMessageSubscriptionToken _progressToken;

        public InstallCommandViewModel(ICommandExecutor executor, ITinyMessengerHub hub, InitializingViewModel initializingViewModel, InstallProgressViewModel installProgressViewModel)
        {
            _executor = executor;
            _hub = hub;
            _initializingViewModel = initializingViewModel;
            _installProgressViewModel = installProgressViewModel;
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
        }
    }
}