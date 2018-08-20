using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AppGet.Commands;
using AppGet.Gui.Views;
using Caliburn.Micro;

namespace AppGet.Gui
{
    public class CaliburnShellViewModel : Conductor<object>
    {

        public CaliburnShellViewModel(IParseOptions optionsParser, IEnumerable<ICommandViewModel> commandViewModels)
        {
            var args = Environment.GetCommandLineArgs();

            try
            {

                if (args.Length <= 1)
                {
                    args = new[]
                    {
                        "appget://install/slack"
                    };
                }

                var option = optionsParser.Parse(args.Last());


                var viewModel = commandViewModels.First(c => c.CanHandle(option));
                ActivateItem(viewModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string MessageTextBlock { get { return "Hi Caliburn!"; } }

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