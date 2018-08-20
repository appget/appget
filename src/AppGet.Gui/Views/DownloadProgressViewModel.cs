using AppGet.Commands;
using AppGet.Extensions;
using AppGet.Gui.Framework;
using AppGet.Infrastructure.Events;
using AppGet.ProgressTracker;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public interface ICommandViewModel : IScreen
    {
        bool CanHandle(AppGetOption options);
    }

    public class DownloadProgressViewModel : Screen
    {
        private readonly ITinyMessengerHub _hub;
        private TinyMessageSubscriptionToken _progressToken;
        private decimal _currentProgressState;

        public DownloadProgressViewModel(ITinyMessengerHub hub)
        {
            _hub = hub;
        }


        private void InstallProgressViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            _progressToken?.Dispose();
        }

        private void InstallProgressViewModel_Activated(object sender, ActivationEventArgs e)
        {
            _progressToken = _hub.Subscribe<GenericTinyMessage<ProgressState>>(OnProgressUpdated);
        }


        private void OnProgressUpdated(GenericTinyMessage<ProgressState> e)
        {
            var newPercent = e.Content.GetPercentCompleted();

            if (newPercent < 99 && _currentProgressState + 1 > newPercent)
            {
                return;
            }

            ProgressStatus = $"Downloading {AppSession.CurrentManifest.Name} {AppSession.CurrentManifest.Version}";

            _currentProgressState = newPercent;
            Progress = (long)newPercent;

            DetailedStatus = e.Content.Value.ToFileSize() + " / " + e.Content.MaxValue.ToFileSize();
        }

        protected override void OnInitialize()
        {
            ProgressStatus = $"Initializing";
            Activated += InstallProgressViewModel_Activated;
            Deactivated += InstallProgressViewModel_Deactivated;
        }

        public string ProgressStatus { get; private set; }
        public long Progress { get; private set; }

        public string DetailedStatus { get; private set; }

    }
}