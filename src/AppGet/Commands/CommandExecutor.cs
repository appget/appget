using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(Arguments arguments);
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<ICommand> _consoleCommands;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CommandExecutor(IEnumerable<ICommand> consoleCommands)
        {
            _consoleCommands = consoleCommands.ToList();
        }

        public void ExecuteCommand(Arguments arguments)
        {
            var commandHandler = _consoleCommands.SingleOrDefault(c => c.CommandName.ToLower() == arguments.Command);

            if (commandHandler == null)
            {
                throw new UnknownCommandException(arguments.Command);
            }

            Logger.Debug(commandHandler.StartMessage);

            var stopwatch = Stopwatch.StartNew();
            commandHandler.Execute(arguments);
            stopwatch.Stop();

            Logger.Info("{0} duration: {1}s", commandHandler.CompletedMessage, stopwatch.Elapsed.TotalSeconds);
        }
    }
}