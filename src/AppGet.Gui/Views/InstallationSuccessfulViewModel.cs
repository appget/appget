using AppGet.Gui.Controls;
using AppGet.Gui.Framework;
using AppGet.Gui.Views.Shared;
using JetBrains.Annotations;

namespace AppGet.Gui.Views
{
    [UsedImplicitly]
    public class InstallationSuccessfulViewModel : DialogViewModel
    {
        protected override void OnActivate()
        {
            var manifest = AppSession.CurrentManifest;
            var nameVersion = $"{manifest.Name} {manifest.Version}";

            this.HeaderViewModel = new DialogHeaderViewModel(nameVersion, "Installed Successfully", "check", Accents.Success);
            base.OnActivate();
        }
    }
}