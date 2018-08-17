using AppGet.Commands;
using AppGet.Gui.Views;
using Caliburn.Micro;

namespace AppGet.Gui
{
    public class ShellViewModel : Conductor<object>
    {
        private readonly IParseOptions _optionsParser;
        private readonly ICommandExecutor _commandExecutor;

        public ShellViewModel(IParseOptions optionsParser, ICommandExecutor commandExecutor, InstallProgressViewModel installProgressViewModel)
        {
            _optionsParser = optionsParser;
            _commandExecutor = commandExecutor;
            ActivateItem(installProgressViewModel);
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