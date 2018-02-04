using System;
using System.Diagnostics;
using AppGet.AppData;
using AppGet.Commands;
using AppGet.Exceptions;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using AppGet.Options;
using NLog;

namespace AppGet
{
    public static class Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            var result = Run(args);

            while (Debugger.IsAttached)
            {
                Run(args);
            }

            return result;
        }


        private static int Run(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                {
                    args = TakeArgsFromInput();
                }

                LogConfigurator.ConfigureLogger();

                var container = ContainerBuilder.Build();

                var optionsService = container.Resolve<IParseOptions>();
                var options = optionsService.Parse(args);

                if (options.Verbose)
                {
                    LogConfigurator.EnableVerboseLogging();
                }

                container.Resolve<IAppDataService>().EnsureAppDataDirectoryExists();

                var commandProcessor = container.Resolve<CommandExecutor>();
                commandProcessor.ExecuteCommand(options);

                return 0;
            }
            catch (UnknownCommandException)
            {
                var helpGenerator = new HelpGenerator();
                var helpText = helpGenerator.GenerateHelp();

                Console.WriteLine(helpText);
                return 0;
            }
            catch (AppGetException ex)
            {
                Logger.Error(ex.Message);
                return 1;
            }
            catch (NotImplementedException ex)
            {
                Logger.Error(ex.Message);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                return 1;
            }
        }

        private static string[] TakeArgsFromInput()
        {
            Console.WriteLine("In debug mode. Please enter arguments");
            var input = Console.ReadLine();
            return input.Split(' ');
        }
    }
}