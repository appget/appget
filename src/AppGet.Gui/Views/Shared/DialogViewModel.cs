using AppGet.Gui.Controls;
using Caliburn.Micro;

namespace AppGet.Gui.Views.Shared
{
    public class DialogViewModel : Conductor<DialogHeaderViewModel>
    {
        public DialogHeaderViewModel HeaderViewModel { get; }

        public DialogViewModel(DialogHeaderViewModel headerViewModel)
        {
            HeaderViewModel = headerViewModel;
        }

        protected override void OnActivate()
        {
            ActivateItem(HeaderViewModel);
        }
    }
}