using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Options;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(AppGetOption options);
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly Logger _logger;
        private readonly List<ICommandHandler> _consoleCommands;


        public CommandExecutor(IEnumerable<ICommandHandler> consoleCommands, Logger logger)
        {
            _logger = logger;
            _consoleCommands = consoleCommands.ToList();
        }

        public void ExecuteCommand(AppGetOption arguments)
        {
            var commandHandler = _consoleCommands.FirstOrDefault(c => c.CanExecute(arguments));

            if (commandHandler == null)
            {
                throw new UnknownCommandException(arguments.CommandName);
            }

            _logger.Debug("Starting command [{0}]", arguments.CommandName);
            var stopwatch = Stopwatch.StartNew();
            commandHandler.Execute(arguments);
            stopwatch.Stop();
            Console.WriteLine();
            _logger.Debug("Completed command [{0}]. duration: {1:N}s", arguments.CommandName, stopwatch.Elapsed.TotalSeconds);
        }
    }
}