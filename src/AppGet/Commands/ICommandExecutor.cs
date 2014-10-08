using AppGet.Options;

namespace AppGet.Commands
{
    public interface ICommandHandler
    {
        bool CanExecute(CommandOptions commandOptions);

        void Execute(CommandOptions commandOptions);
    }
}