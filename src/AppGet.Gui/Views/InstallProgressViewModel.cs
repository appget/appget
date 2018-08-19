using System.Threading.Tasks;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.Infrastructure.Events;
using AppGet.ProgressTracker;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public interface ICommandViewModel : IScreen
    {
        bool CanHandle(AppGetOption options);
    }

    public class InstallProgressViewModel : CommandViewModel<InstallOptions>
    {
        private readonly ICommandExecutor _executor;
        private readonly ITinyMessengerHub _hub;
        private TinyMessageSubscriptionToken _progressToken;

        public InstallProgressViewModel(ICommandExecutor executor, ITinyMessengerHub hub)
        {
            _executor = executor;
            _hub = hub;
            Activated += InstallProgressViewModel_Activated;
            Deactivated += InstallProgressViewModel_Deactivated;
        }


        private void InstallProgressViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            _progressToken?.Dispose();
        }

        private void InstallProgressViewModel_Activated(object sender, ActivationEventArgs e)
        {
            _progressToken = _hub.Subscribe<GenericTinyMessage<ProgressState>>(OnProgressUpdated);
            Task.Run(() => _executor.ExecuteCommand(Options));
        }

        private void OnProgressUpdated(GenericTinyMessage<ProgressState> obj)
        {
            ProgressMaximum = obj.Content.MaxValue;
            Progress = obj.Content.Value;
        }

        protected override void OnInitialize()
        {
            ProgressStatus = $"Installing {Options.Package}";
        }

        public string ProgressStatus { get; private set; }

        public long Progress { get; private set; }
        public long ProgressMaximum { get; private set; }

        public bool ProgressIndeterminate => Progress == 0;
    }
}