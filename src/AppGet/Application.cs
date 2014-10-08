using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppGet.Download;
using AppGet.Exceptions;
using NLog;
using NLog.Config;
using NLog.Targets;
using TinyIoC;

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
                ConfigureLogger();

                var options = Options.Parse(args);

                var container = new TinyIoCContainer();
                container.AutoRegister();

                RegisterDownloadClients(container);


                return 0;
            }
            catch (AppGetException ex)
            {
                Logger.Error(ex.Message);
                return 1;
            }
        }

        private static void ConfigureLogger()
        {
            LogManager.Configuration = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            var rule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            LogManager.Configuration.AddTarget("console", consoleTarget);
            LogManager.Configuration.LoggingRules.Add(rule);

            LogManager.ReconfigExistingLoggers();
        }

        private static void RegisterDownloadClients(TinyIoCContainer container)
        {
            container.RegisterMultiple<IDownloadClient>(new List<Type> { typeof(HttpDownloadClient) });
        }
    }
}