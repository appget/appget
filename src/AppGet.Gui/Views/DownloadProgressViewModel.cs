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

    public class DownloadProgressViewModel : Screen, Infrastructure.Events.IHandle<ProgressChangeEvent>
    {
        private readonly IEventHub _hub;
        private decimal _currentProgressState;

        public DownloadProgressViewModel(IEventHub hub)
        {
            _hub = hub;
        }

        protected override void OnInitialize()
        {
            ProgressStatus = $"Initializing";
        }

        public string ProgressStatus { get; private set; }
        public long Progress { get; private set; }

        public string DetailedStatus { get; private set; }

        public void Handle(ProgressChangeEvent message)
        {
            var progress = message.Progress;

            var newPercent = progress.GetPercentCompleted();

            if (newPercent < 99 && _currentProgressState + 1 > newPercent)
            {
                return;
            }

            ProgressStatus = $"Downloading {AppSession.CurrentManifest.Name} {AppSession.CurrentManifest.Version}";

            _currentProgressState = newPercent;
            Progress = (long)newPercent;

            DetailedStatus = progress.Value.ToFileSize() + " / " + progress.MaxValue.ToFileSize();
        }
    }
}