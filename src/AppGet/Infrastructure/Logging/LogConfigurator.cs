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
                Layout = new SimpleLayout("> ${message}")
            };
            _rule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
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