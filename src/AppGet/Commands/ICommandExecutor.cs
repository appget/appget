namespace AppGet.Commands
{
    public interface ICommandHandler
    {
        bool CanExecute(AppGetOption commandOptions);

        void Execute(AppGetOption commandOptions);
    }
}