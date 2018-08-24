using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace AppGet.Infrastructure.Logging
{
    public static class LogConfigurator
    {
        private static LoggingRule _consoleRule;

        public static readonly Layout FriendlyLayout = new SimpleLayout("${trim-whitespace:${message} ${exception:format=message}}");
        public static readonly Layout DetailedLayout = new SimpleLayout("[${Logger}] ${trim-whitespace:${message} ${exception:format=ToString}}");

        static LogConfigurator()
        {
            LogManager.Configuration = new LoggingConfiguration();
        }

        public static void EnableSentryTarget(string dsn)
        {
            var sentryTarget = new SentryTarget(dsn);

            var rule = new LoggingRule("*", LogLevel.Trace, sentryTarget);
            LogManager.Configuration.AddTarget("sentry", sentryTarget);

            LogManager.Configuration.LoggingRules.Add(rule);
        }

        public static void EnableFileTarget()
        {
            var fileTarget = new FileTarget
            {
                Layout =
                    new SimpleLayout("[${level}] ${longdate} ${logger} ${message}${exception:format=ToString}"),
                FileName = "${basedir}/logs.txt"
            };


            var rule = new LoggingRule("*", LogLevel.Trace, fileTarget);
            LogManager.Configuration.AddTarget("fileTarget", fileTarget);

            LogManager.Configuration.LoggingRules.Add(rule);

            LogManager.ReconfigExistingLoggers();
        }

        public static void EnableConsoleTarget(Layout layout)
        {
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = layout,
            };

            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,4}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)",
                CompileRegex = true,
                IgnoreCase = true,
                ForegroundColor = ConsoleOutputColor.DarkCyan
            });

            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "\\B\\[(\\w|-)+:\\w+]\\B",
                CompileRegex = true,
                ForegroundColor = ConsoleOutputColor.DarkMagenta
            });

            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "PASSED",
                WholeWords = true,
                CompileRegex = true,
                IgnoreCase = false,
                ForegroundColor = ConsoleOutputColor.DarkGreen
            });

            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = "FAILED",
                WholeWords = true,
                CompileRegex = true,
                IgnoreCase = false,
                ForegroundColor = ConsoleOutputColor.DarkRed
            });

            // package:tag
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = @"[a-z0-9-]{2,}\:[a-z0-9\.]+",
                WholeWords = true,
                CompileRegex = true,
                IgnoreCase = false,
                ForegroundColor = ConsoleOutputColor.Green
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.NoChange,
                ConsoleOutputColor.NoChange));

            _consoleRule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            LogManager.Configuration.AddTarget("console", consoleTarget);
            LogManager.Configuration.LoggingRules.Add(_consoleRule);

            return;
        }

        public static void EnableVerboseLogging()
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevel(LogLevel.Debug);
                rule.EnableLoggingForLevel(LogLevel.Trace);
            }

            LogManager.ReconfigExistingLoggers();
        }
    }
}