namespace AppGet.Commands
{
    public interface ICommand
    {
        string CommandName { get; }
        string CommandDescription { get; }

        string StartMessage { get; }
        string CompletedMessage { get; }

        void Execute(Arguments arguments);
    }
}