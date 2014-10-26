using AppGet.Options;

namespace AppGet.Commands
{
    public interface ICommandHandler
    {
        bool CanExecute(AppGetOption packageCommandOptions);

        void Execute(AppGetOption packageCommandOptions);
    }
}