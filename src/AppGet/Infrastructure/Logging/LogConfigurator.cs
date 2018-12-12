using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace AppGet.Infrastructure.Logging
{
    public static class LogConfigurator
    {
        public static readonly Layout FriendlyLayout = new SimpleLayout("${trim-whitespace:${message} ${exception:format=message}}");
        public static readonly Layout DetailedLayout = new SimpleLayout("${date:format=MM-dd HH\\:mm\\:ss.fff}: [${Logger}] ${trim-whitespace:${message} ${exception:format=ToString}}");

        static LogConfigurator()
        {
            LogManager.Configuration = new LoggingConfiguration();
        }

        private static void RegisterTarget(Target target, LogLevel level)
        {
            var name = target.GetType().Name;

            if (LogManager.Configuration.FindTargetByName(name) != null) return;


            var rule = new LoggingRule("*", level, target);
            LogManager.Configuration.AddTarget(name, target);
            LogManager.Configuration.LoggingRules.Add(rule);
            LogManager.ReconfigExistingLoggers();
        }

        public static void EnableSentryTarget(string dsn)
        {
            var sentryTarget = new SentryTarget(dsn);
            RegisterTarget(sentryTarget, LogLevel.Trace);
        }

        public static void EnableFileTarget(LogLevel level, int maxArchiveFiles = 7)
        {
            var fileTarget = new FileTarget
            {
                Layout = new SimpleLayout("${longdate} ${processid}:${threadid} [${level:padding=5}]  ${logger}: ${message}${exception:format=ToString}"),
                FileName = "${specialfolder:folder=LocalApplicationData}/AppGet/Logs/${processname}.log",
                ArchiveFileName = "${specialfolder:folder=LocalApplicationData}/AppGet/Logs/${processname}.{#}.log",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                MaxArchiveFiles = maxArchiveFiles,
                KeepFileOpen = false,
                ConcurrentWrites = false
            };

            RegisterTarget(fileTarget, level);
        }

        public static void EnableConsoleTarget(Layout layout, LogLevel level)
        {
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = layout
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
                Regex = @"[a-z0-9-]{2,}\:[a-z0-9\.]+\s",
                WholeWords = true,
                CompileRegex = true,
                IgnoreCase = false,
                ForegroundColor = ConsoleOutputColor.Green
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.NoChange,
                ConsoleOutputColor.NoChange));

            RegisterTarget(consoleTarget, level);
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