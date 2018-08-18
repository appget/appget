using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AppGet.Infrastructure.Logging;
using NLog;

namespace AppGet.Commands
{
    public interface ICommandExecutor
    {
        Task ExecuteCommand(string[] args);
        Task ExecuteCommand(AppGetOption option);
    }

    public class CommandExecutor : ICommandExecutor
    {
        private readonly Func<string, ICommandHandler> _handlerFactory;
        private readonly IParseOptions _optionParser;
        private readonly Logger _logger;

        public CommandExecutor(Func<String, ICommandHandler> handlerFactory, IParseOptions optionParser, Logger logger)
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

        public async Task ExecuteCommand(AppGetOption option)
        {
            if (option.Verbose)
            {
                LogConfigurator.EnableVerboseLogging();
            }

            var commandHandler = _handlerFactory.Invoke(option.GetType().FullName);

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