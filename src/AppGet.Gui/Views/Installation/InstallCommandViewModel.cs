using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.Infrastructure.Events;
using Caliburn.Micro;

namespace AppGet.Gui.Views.Installation
{
    public class InstallCommandViewModel : CommandViewModel<InstallOptions>
    {
        private readonly ICommandExecutor _executor;
        private readonly ITinyMessengerHub _hub;
        private readonly InitializingViewModel _initializingViewModel;
        private TinyMessageSubscriptionToken _progressToken;

        public InstallCommandViewModel(ICommandExecutor executor, ITinyMessengerHub hub, InitializingViewModel initializingViewModel)
        {
            _executor = executor;
            _hub = hub;
            _initializingViewModel = initializingViewModel;
        }

        protected override void OnActivate()
        {
            ActivateItem(_initializingViewModel);
        }

        protected override void OnInitialize()
        {

        }

    }
}