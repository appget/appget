using AppGet.Commands.Install;
using AppGet.Gui.Controls;
using AppGet.Gui.Framework;
using AppGet.Gui.Views.Shared;
using JetBrains.Annotations;

namespace AppGet.Gui.Views
{
    [UsedImplicitly]
    public class InstallationConsentViewModel : DialogViewModel
    {
        protected override void OnActivate()
        {
            var installOptions = (InstallOptions)AppSession.Options;


            HeaderViewModel = new DialogHeaderViewModel($"Install {installOptions.Package}", $"Are you sure you want to install {installOptions.Package}?", "question-circle", Accents.Info);
            base.OnActivate();
        }
    }
}