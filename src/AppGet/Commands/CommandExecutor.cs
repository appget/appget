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
        void ExecuteCommand(AppGetOption option);
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

        public void ExecuteCommand(AppGetOption option)
        {
            var commandHandler = _consoleCommands.FirstOrDefault(c => c.CanExecute(option));

            if (commandHandler == null)
            {
                throw new UnknownCommandException(option);
            }

            _logger.Debug("Starting command [{0}]", option.CommandName);
            var stopwatch = Stopwatch.StartNew();
            commandHandler.Execute(option);
            stopwatch.Stop();
            Console.WriteLine();
            _logger.Debug("Completed command [{0}]. duration: {1:N}s", option.CommandName, stopwatch.Elapsed.TotalSeconds);
        }
    }
}