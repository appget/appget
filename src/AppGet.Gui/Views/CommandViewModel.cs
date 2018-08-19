using AppGet.Commands;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public abstract class CommandViewModel<T> : Screen, ICommandViewModel where T : AppGetOption
    {
        public T Options { get; private set; }

        public bool CanHandle(AppGetOption options)
        {
            Options = options as T;
            return options != null;
        }
    }
}