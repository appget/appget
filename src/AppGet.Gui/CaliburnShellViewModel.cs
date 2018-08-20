using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AppGet.Commands;
using AppGet.Gui.Views;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace AppGet.Gui
{
    [UsedImplicitly]
    public class CaliburnShellViewModel : Conductor<IScreen>
    {
        public CaliburnShellViewModel(IParseOptions optionsParser, IEnumerable<ICommandViewModel> commandViewModels)
        {
            Task.Run(() =>
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
            });
        }
    }
}