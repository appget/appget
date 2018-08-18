using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AppGet.Commands;
using AppGet.Gui.Views;
using Caliburn.Micro;

namespace AppGet.Gui
{
    public class ShellViewModel : Conductor<object>
    {

        public ShellViewModel(IParseOptions optionsParser, IEnumerable<ICommandViewModel> commandViewModels)
        {
            var args = Environment.GetCommandLineArgs();

            try
            {

                var option = optionsParser.Parse(args.Last());


                var viewModel = commandViewModels.First(c => c.CanHandle(option));
                ActivateItem(viewModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //        protected override void OnActivate()
        //        {
        //            var args = Environment.GetCommandLineArgs();
        //            var option = (InstallOptions)_optionsParser.Parse(args.Last());
        //
        //            _commandExecutor.ExecuteCommand(option);
        //
        //            base.OnActivate();
        //        }
    }
}