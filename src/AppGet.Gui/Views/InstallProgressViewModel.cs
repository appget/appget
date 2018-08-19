//using System.Threading.Tasks;
//using AppGet.Commands;
//using AppGet.Commands.Install;
//using AppGet.Infrastructure.Events;
//using AppGet.ProgressTracker;
//using Caliburn.Micro;
//
//namespace AppGet.Gui.Views
//{

using AppGet.Commands;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public interface ICommandViewModel : IScreen
    {
        bool CanHandle(AppGetOption options);
    }
}
//
//    public class InstallProgressViewModel : CommandViewModel<InstallOptions>
//    {
//        private readonly ICommandExecutor _executor;
//        private readonly ITinyMessengerHub _hub;
//        private TinyMessageSubscriptionToken _progressToken;
//        private decimal _currentProgressState;
//
//        public InstallProgressViewModel(ICommandExecutor executor, ITinyMessengerHub hub)
//        {
//            _executor = executor;
//            _hub = hub;
//        }
//
//
//        private void InstallProgressViewModel_Deactivated(object sender, DeactivationEventArgs e)
//        {
//            _progressToken?.Dispose();
//        }
//
//        private void InstallProgressViewModel_Activated(object sender, ActivationEventArgs e)
//        {
//            _progressToken = _hub.Subscribe<GenericTinyMessage<ProgressState>>(OnProgressUpdated);
//            _progressToken = _hub.Subscribe<StatusUpdateEvent>(OnStatusUpdated);
//            Task.Run(() => _executor.ExecuteCommand(Options));
//        }
//
//        private void OnStatusUpdated(StatusUpdateEvent e)
//        {
//            ProgressStatus = e.Message;
//        }
//
//        private void OnProgressUpdated(GenericTinyMessage<ProgressState> e)
//        {
//            var newPercent = e.Content.GetPercentCompleted();
//
//            if (newPercent < 99 && _currentProgressState + 1 > newPercent)
//            {
//                return;
//            }
//
//            ProgressStatus = $"Downloading Installer";
//
//            _currentProgressState = newPercent;
//            ProgressMaximum = 100;
//            Progress = (long)newPercent;
//        }
//
//        protected override void OnInitialize()
//        {
//            ProgressStatus = $"Initializing";
//            Activated += InstallProgressViewModel_Activated;
//            Deactivated += InstallProgressViewModel_Deactivated;
//        }
//
//        public string ProgressStatus { get; private set; }
//
//        public long Progress { get; private set; }
//        public long ProgressMaximum { get; private set; }
//
//        public bool ProgressIndeterminate => Progress == 0;
//    }
//}