using AppGet.Gui.Framework;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace AppGet.Gui.Views
{
    [UsedImplicitly]
    public class InstallationSuccessfulViewModel : Screen
    {
        protected override void OnInitialize()
        {
            var manifest = AppSession.CurrentManifest;
            this.Message = $"{manifest.Name} {manifest.Version}";
            this.LaunchButton = $"Launch {manifest.Name}";
        }

        public string Message { get; private set; }

        public string LaunchButton { get; private set; }

    }
}