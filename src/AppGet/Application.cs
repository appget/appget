using System;
using System.Diagnostics;
using System.Reflection;
using AppGet.Commands;
using AppGet.Composition;
using AppGet.Exceptions;
using AppGet.Options;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace AppGet
{
    public static class Application
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static TimeSpan ApplicationLifetime
        {
            get
            {
                return Stopwatch.Elapsed;
            }
        }

        public static int Main(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                {
                    args = TakeArgsFromInput();
                }


                ConfigureLogger();

                var container = ContainerBuilder.Build();

                var optionsService = container.Resolve<IOptionsService>();
                var options = optionsService.Parse(args);

                var commandProcessor = container.Resolve<CommandExecutor>();
                commandProcessor.ExecuteCommand(options);


                return 0;
            }
            catch (AppGetException ex)
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

        private static void ConfigureLogger()
        {
            LogManager.Configuration = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = new SimpleLayout("> ${message}")
            };
            var rule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            LogManager.Configuration.AddTarget("console", consoleTarget);
            LogManager.Configuration.LoggingRules.Add(rule);

            LogManager.ReconfigExistingLoggers();
        }
    }
}