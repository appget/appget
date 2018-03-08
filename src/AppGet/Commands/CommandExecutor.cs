using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        Task ExecuteCommand(AppGetOption option);
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly Logger _logger;
        private readonly List<ICommandHandler> _commandHandlers;


        public CommandExecutor(IEnumerable<ICommandHandler> commandHandlers, Logger logger)
        {
            _logger = logger;
            _commandHandlers = commandHandlers.ToList();
        }

        public async Task ExecuteCommand(AppGetOption option)
        {
            var commandHandler = _commandHandlers.FirstOrDefault(c => c.CanExecute(option));

            if (commandHandler == null)
            {
                throw new UnknownCommandException(option);
            }

            _logger.Debug("Starting command [{0}]", option.CommandName);
            var stopwatch = Stopwatch.StartNew();
            await commandHandler.Execute(option);
            stopwatch.Stop();
            Console.WriteLine();
            _logger.Debug("Completed command [{0}]. took: {1:N}s", option.CommandName, stopwatch.Elapsed.TotalSeconds);
        }
    }
}