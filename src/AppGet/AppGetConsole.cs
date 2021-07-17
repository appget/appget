using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Commands;
using AppGet.Exceptions;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using AppGet.PackageRepository;
using AppGet.Update;
using DryIoc;
using NLog;

namespace AppGet
{
    public static class AppGetConsole
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            var result = Run(args).Result;

            while (Debugger.IsAttached)
            {
                var existCode = Run(new string[0]).Result;
                Console.WriteLine("Exit Code: " + existCode);
            }

            return result;
        }

        private static async Task<int> Run(string[] args)
        {
            IAppGetUpdateService updatedService = null;

            try
            {
                var container = ContainerBuilder.Container;

                LogConfigurator.EnableConsoleTarget(LogConfigurator.FriendlyLayout, LogLevel.Info);
                LogConfigurator.EnableFileTarget(LogLevel.Trace);

                try
                {
                    updatedService = container.Resolve<IAppGetUpdateService>();
                    updatedService.Start();
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Failed to initilize update service.");
                }

                if (Debugger.IsAttached && !args.Any())
                {
                    args = TakeArgsFromInput().SkipWhile(c => c.ToLowerInvariant() == "appget").ToArray();
                }

                Logger.Warn("Thank you for all your support!");
                Logger.Warn("AppGet is no longer maintained and is shutting down. You should probably look into WinGet ;)");
                Logger.Warn("See https://appget.net for more details.");

                return 404;
            }
            catch (CommandLineParserException)
            {
                return 1;
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

            return input?.Split(' ');
        }
    }
}
