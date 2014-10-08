using System;
using System.Runtime.Remoting;

namespace AppGet.Commands
{
    public interface ICommandHandler
    {
        string CommandName { get; }
        string CommandDescription { get; }

        string StartMessage { get; }
        string CompletedMessage { get; }

        bool CanExecute(CommandOptions commandOptions);

        void Execute(CommandOptions commandOptions);
    }
}