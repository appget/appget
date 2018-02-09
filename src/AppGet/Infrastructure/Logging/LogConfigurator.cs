using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace AppGet.Infrastructure.Logging
{
    public static class LogConfigurator
    {
        private static LoggingRule _rule;

        public static void ConfigureLogger()
        {
            LogManager.Configuration = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = new SimpleLayout("${message} ${exception:format=message}"),
            };


            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,4}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)",
                CompileRegex = true,
                ForegroundColor = ConsoleOutputColor.Blue
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange));

            _rule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            LogManager.Configuration.AddTarget("console", consoleTarget);
            LogManager.Configuration.LoggingRules.Add(_rule);

            LogManager.ReconfigExistingLoggers();
        }

        public static void EnableVerboseLogging()
        {
            _rule.EnableLoggingForLevel(LogLevel.Debug);
            _rule.EnableLoggingForLevel(LogLevel.Trace);
            LogManager.ReconfigExistingLoggers();
        }
    }
}