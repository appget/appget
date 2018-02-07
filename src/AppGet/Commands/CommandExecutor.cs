using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(AppGetOption option);
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly AppGetOption _option;
        private readonly Logger _logger;
        private readonly List<ICommandHandler> _commandHandlers;


        public CommandExecutor(IEnumerable<ICommandHandler> commandHandlers,AppGetOption option, Logger logger)
        {
            _option = option;
            _logger = logger;
            _commandHandlers = commandHandlers.ToList();
        }

        public void ExecuteCommand(AppGetOption option)
        {
            var commandHandler = _commandHandlers.FirstOrDefault(c => c.CanExecute(option));

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