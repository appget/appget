using AppGet.Commands;
using Caliburn.Micro;

namespace AppGet.Gui.Views
{
    public abstract class CommandViewModel<T> : Conductor<IScreen>, ICommandViewModel where T : AppGetOption
    {
        protected T Options { get; private set; }

        public bool CanHandle(AppGetOption options)
        {
            Options = options as T;
            return options != null;
        }
    }
}