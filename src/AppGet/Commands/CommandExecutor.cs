using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(CommandOptions options);
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<ICommandHandler> _consoleCommands;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CommandExecutor(IEnumerable<ICommandHandler> consoleCommands)
        {
            _consoleCommands = consoleCommands.ToList();
        }

        public void ExecuteCommand(CommandOptions arguments)
        {
            var commandHandler = _consoleCommands.FirstOrDefault(c => c.CanExecute(arguments));

            if (commandHandler == null)
            {
                throw new UnknownCommandException(arguments.CommandName);
            }

            Logger.Debug(commandHandler.StartMessage);

            var stopwatch = Stopwatch.StartNew();
            commandHandler.Execute(arguments);
            stopwatch.Stop();

            Logger.Info("{0} duration: {1}s", commandHandler.CompletedMessage, stopwatch.Elapsed.TotalSeconds);
        }
    }
}