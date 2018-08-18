using System.Windows;
using AppGet.Commands;
using AppGet.Commands.Install;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public interface ICommandViewModel : IScreen
    {
        bool CanHandle(AppGetOption options);
    }

    public abstract class CommandViewModel<T> : Screen, ICommandViewModel where T : AppGetOption
    {
        public T Options { get; private set; }

        public bool CanHandle(AppGetOption options)
        {
            Options = options as T;
            return options != null;
        }
    }

    public class InstallProgressViewModel : CommandViewModel<InstallOptions>
    {
        protected override void OnInitialize()
        {
            ProgressMaximum = 75;
            //            Progress = 50;
            ProgressStatus = $"Installing {Options.Package}";
        }

        public string ProgressStatus { get; private set; }

        public int Progress { get; private set; }
        public int ProgressMaximum { get; private set; }

        public bool ProgressIndeterminate => Progress == 0;
    }
}