using AppGet.Gui.Controls;
using Caliburn.Micro;

namespace AppGet.Gui.Views.Shared
{
    public class DialogViewModel : Conductor<DialogHeaderViewModel>
    {
        public DialogHeaderViewModel HeaderViewModel { get; protected set; }

        public DialogViewModel(DialogHeaderViewModel headerViewModel)
        {
            HeaderViewModel = headerViewModel;
        }

        protected DialogViewModel()
        {

        }

        protected override void OnActivate()
        {
            ActivateItem(HeaderViewModel);
        }
    }
}