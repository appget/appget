using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CommandLine;
using AppGet.Commands;
using AppGet.Exceptions;
using AppGet.PackageRepository;
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
            try
            {
                if (Debugger.IsAttached && !args.Any())
                {
                    args = TakeArgsFromInput();
                }

                await Application.Execute(args);
                return 0;
            }
            catch (CommandLineParserException e)
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