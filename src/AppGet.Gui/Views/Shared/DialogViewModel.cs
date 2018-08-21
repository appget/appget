using System;
using AppGet.Gui.Controls;
using Caliburn.Micro;

namespace AppGet.Gui.Views.Shared
{
    public static class DialogFactory
    {
        public static DialogViewModel CreateDialog(this Exception ex)
        {
            var headerVm = new DialogHeaderViewModel(ex.GetType().Name.Replace("Exception", ""), ex.Message, "sad-cry", Accents.Error);
            return new DialogViewModel(headerVm);
        }
    }


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