using AppGet.Gui.Framework;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public class InstallingViewModel : Screen
    {
        protected override void OnInitialize()
        {
            var manifest = AppSession.CurrentManifest;
            this.Message = $"Installing {manifest.Name} {manifest.Version}";
        }

        public string Message { get; private set; }
    }
}