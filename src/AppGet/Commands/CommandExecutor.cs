using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AppGet.Infrastructure.Logging;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        Task ExecuteCommand(string[] args);
        Task ExecuteCommand<T>(T option) where T : AppGetOption;
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly IEnumerable<ICommandHandler> _handlerFactory;
        private readonly IParseOptions _optionParser;
        private readonly Logger _logger;

        public CommandExecutor(IEnumerable<ICommandHandler> handlerFactory, IParseOptions optionParser, Logger logger)
        {
            _handlerFactory = handlerFactory;
            _optionParser = optionParser;
            _logger = logger;
        }


        public async Task ExecuteCommand(string[] args)
        {
            var options = _optionParser.Parse(args);
            await ExecuteCommand(options);
        }

        public async Task ExecuteCommand<T>(T option) where T : AppGetOption
        {
            if (option.Verbose)
            {
                LogConfigurator.EnableVerboseLogging();
            }

            foreach (var handler in _handlerFactory)
            {
                if (handler is ICommandHandler<T> h)
                {
                    _logger.Debug("Starting command [{0}]", option.CommandName);
                    var stopwatch = Stopwatch.StartNew();
                    await h.Execute(option);
                    stopwatch.Stop();
                    Console.WriteLine();
                    _logger.Debug("Completed command [{0}]. took: {1:N}s", option.CommandName, stopwatch.Elapsed.TotalSeconds);
                    return;
                }
            }

            throw new UnknownCommandException(option);

        }
    }
}