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
        }

        public string Message { get; private set; }

        public void Close()
        {

        }

    }
}