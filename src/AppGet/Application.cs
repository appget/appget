using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.AppData;
using AppGet.CommandLine;
using AppGet.Commands;
using AppGet.Exceptions;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using AppGet.PackageRepository;
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
                Run(new string[0]);
            }

            return result;
        }

        private static int Run(string[] args)
        {
            return Execute(args).Result;
        }

        private static async Task<int> Execute(string[] args)
        {
            IAppGetUpdateService updatedService = null;
            try
            {
                if (Debugger.IsAttached && !args.Any())
                {
                    args = TakeArgsFromInput();
                }

                LogConfigurator.ConfigureLogger();

                var container = ContainerBuilder.Build();

                updatedService = container.Resolve<IAppGetUpdateService>();
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

                return 0;
            }
            catch (PackageNotFoundException e)
            {
                Logger.Warn(e.Message);

                if (e.Similar.Any())
                {
                    Console.WriteLine("");
                    Console.WriteLine("Suggestions:");
                    e.Similar.ShowTable();
                }

                return 1;
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
            finally
            {
                if (updatedService != null)
                {
                    await updatedService.Commit();
                }
            }
        }

        private static string[] TakeArgsFromInput()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("In debug mode. Please enter arguments");
            var input = Console.ReadLine();

            return input.Split(' ');
        }
    }
}