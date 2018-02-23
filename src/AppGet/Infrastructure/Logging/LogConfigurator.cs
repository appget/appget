using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace AppGet.Infrastructure.Logging
{
    public static class LogConfigurator
    {
        private static LoggingRule _consoleRule;

        public static void ConfigureLogger(string[] args)
        {
            LogManager.Configuration = new LoggingConfiguration();

            BuildConsoleTarget();


            BuildSentryTarget(args);



            LogManager.ReconfigExistingLoggers();
        }

        private static void BuildSentryTarget(string[] args)
        {
            var sentryTarget = new SentryTarget(args);

            var rule = new LoggingRule("*", LogLevel.Trace, sentryTarget);
            LogManager.Configuration.AddTarget("sentry", sentryTarget);

            LogManager.Configuration.LoggingRules.Add(rule);
        }


        private static ColoredConsoleTarget BuildConsoleTarget()
        {
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = new SimpleLayout("${trim-whitespace:${message} ${exception:format=message}}"),
            };

            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,4}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)",
                CompileRegex = true,
                ForegroundColor = ConsoleOutputColor.DarkCyan
            });


            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "\\B\\[(\\w|-)+:\\w+]\\B",
                CompileRegex = true,
                ForegroundColor = ConsoleOutputColor.DarkMagenta
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange));

            _consoleRule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            LogManager.Configuration.AddTarget("console", consoleTarget);
            LogManager.Configuration.LoggingRules.Add(_consoleRule);

            return consoleTarget;
        }

        public static void EnableVerboseLogging()
        {
            _consoleRule.EnableLoggingForLevel(LogLevel.Debug);
            _consoleRule.EnableLoggingForLevel(LogLevel.Trace);
            LogManager.ReconfigExistingLoggers();
        }
    }
}