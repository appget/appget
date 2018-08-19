using System.Collections.Generic;
using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.Infrastructure.Events;
using AppGet.ProgressTracker;
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


        private void OnDeactivated(object sender, DeactivationEventArgs e)
        {
            _progressToken?.Dispose();
        }

        private void OnActivated(object sender, ActivationEventArgs e)
        {
            ActivateItem(_initializingViewModel);
        }

        protected override void OnInitialize()
        {
            Activated += OnActivated;
            Deactivated += OnDeactivated;
        }

    }
}