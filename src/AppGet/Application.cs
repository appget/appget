using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.AppData;
using AppGet.Commands;
using AppGet.Exceptions;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using AppGet.Update;
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
                Run(new string[0]).Wait();
            }

            return result.Result;
        }


        private static async Task<int> Run(string[] args)
        {
            try
            {
                if (Debugger.IsAttached && !args.Any())
                {
                    args = TakeArgsFromInput();
                }

                LogConfigurator.ConfigureLogger(args);

                var container = ContainerBuilder.Build();

                var updatedService = container.Resolve<IAppGetUpdateService>();
                updatedService.Start();

                var optionsService = container.Resolve<IParseOptions>();
                var options = optionsService.Parse(args);

                if (options == null)
                {
                    return 1;
                }

                if (options.Verbose)
                {
                    LogConfigurator.EnableVerboseLogging();
                }

                container.Resolve<IAppDataService>().EnsureAppDataDirectoryExists();

                var commandExecutor = container.Resolve<ICommandExecutor>();
                await commandExecutor.ExecuteCommand(options);

                updatedService.Commit().Wait();

                return 0;
            }

            catch (AppGetException ex)
            {
                Logger.Error(ex, null);
                return 1;
            }
            catch (NotImplementedException ex)
            {
                Logger.Error(ex, null);
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, null);
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