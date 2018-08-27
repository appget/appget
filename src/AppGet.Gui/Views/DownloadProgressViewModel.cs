using AppGet.Commands;
using AppGet.Extensions;
using AppGet.Gui.Framework;
using AppGet.Infrastructure.Eventing;
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
        private readonly IHub _hub;
        private decimal _currentProgressState;

        public DownloadProgressViewModel(IHub hub)
        {
            _hub = hub;
        }

        private void OnProgressUpdated(ProgressUpdatedEvent e)
        {
            var newPercent = e.GetPercentCompleted();

            if (newPercent < 99 && _currentProgressState + 1 > newPercent)
            {
                return;
            }

            ProgressStatus = $"Downloading {AppSession.CurrentManifest.Name} {AppSession.CurrentManifest.Version}";

            _currentProgressState = newPercent;
            Progress = (long)newPercent;

            DetailedStatus = e.Value.ToFileSize() + " / " + e.MaxValue.ToFileSize();
        }

        protected override void OnInitialize()
        {
            ProgressStatus = "Initializing";

            Activated += (sender, e) =>
            {
                _hub.Subscribe<ProgressUpdatedEvent>(this, OnProgressUpdated);
            };

            Deactivated += (sender, e) =>
            {
                _hub.UnSubscribe(this);
            };
        }

        public string ProgressStatus { get; private set; }
        public long Progress { get; private set; }

        public string DetailedStatus { get; private set; }
    }
}