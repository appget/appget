using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AppGet.Commands;
using AppGet.Gui.Framework;
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

                    //                    if (args.Length <= 1)
                    //                    {
                    //                        args = new[]
                    //                        {
                    //                            "appget://install/vlc",
                    ////                            "appget://install/visual-studio-code"
                    //                        };
                    //                    }

                    var option = optionsParser.Parse(args.Last());

                    AppSession.Options = option;

                    var viewModel = commandViewModels.First(c => c.CanHandle(option));
                    ActivateItem(viewModel);
                }
                catch (CommandLineParserException)
                {
                    Process.Start("https://appget.net/packages");
                    Environment.Exit(1);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }
            });
        }

        public void Close()
        {
            var shell = (IDeactivate)this;
            shell.Deactivate(true);
        }
    }
}