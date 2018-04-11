using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AppGet.HostSystem;
using NLog;
using NLog.Common;
using NLog.Targets;
using SharpRaven;
using SharpRaven.Data;

namespace AppGet.Infrastructure.Logging
{
    [Target("Sentry")]
    public class SentryTarget : TargetWithLayout
    {
        private readonly RavenClient _client;

        private const string DSN = "https://49bc14b2d3484029adcf54e86d5055b0:885dd046962a49ca80064e9670719043@sentry.io/306545";

        private static readonly IDictionary<LogLevel, ErrorLevel> LoggingLevelMap = new Dictionary<LogLevel, ErrorLevel>
        {
            {
                LogLevel.Debug, ErrorLevel.Debug
            },
            {
                LogLevel.Error, ErrorLevel.Error
            },
            {
                LogLevel.Fatal, ErrorLevel.Fatal
            },
            {
                LogLevel.Info, ErrorLevel.Info
            },
            {
                LogLevel.Trace, ErrorLevel.Debug
            },
            {
                LogLevel.Warn, ErrorLevel.Warning
            },
        };

        public SentryTarget()
        {
            _client = new RavenClient(new Dsn(DSN), new JsonPacketFactory(), new SentryRequestFactory(), new SentryUserFactory())
            {
                Compression = true,
                ErrorOnCapture = OnError,
                Release = BuildInfo.Version.ToString(),
                Environment = BuildInfo.IsProduction ? "Production" : "Dev",
            };

            var osInfo = new EnvInfo();

            _client.Tags.Add("culture", Thread.CurrentThread.CurrentCulture.Name);
            _client.Tags.Add("os_name", osInfo.Name);
            _client.Tags.Add("os_version", osInfo.Version.ToString());
            _client.Tags.Add("os_bit", osInfo.Is64BitOperatingSystem ? "64" : "32");
            _client.Tags.Add("args", Environment.CommandLine);
            _client.Tags.Add("runtime", Environment.Version.ToString());
        }

        private void OnError(Exception ex)
        {
            InternalLogger.Error(ex, "Unable to send error to Sentry");
        }

        private static BreadcrumbLevel GetLevel(LogLevel level)
        {
            if (level == LogLevel.Trace || level == LogLevel.Debug) return BreadcrumbLevel.Debug;

            if (level == LogLevel.Info) return BreadcrumbLevel.Info;

            if (level == LogLevel.Warn) return BreadcrumbLevel.Warning;

            if (level == LogLevel.Error) return BreadcrumbLevel.Error;

            return BreadcrumbLevel.Critical;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                var message = logEvent.FormattedMessage;

                if (!string.IsNullOrEmpty(message))
                {
                    _client.AddTrail(new Breadcrumb("log", BreadcrumbType.Navigation)
                    {
                        Level = GetLevel(logEvent.Level),
                        Message = message
                    });
                }

                if (logEvent.Level.Ordinal < LogLevel.Error.Ordinal)
                {
                    return;
                }

                var extras = logEvent.Properties.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                var ex = logEvent.Exception;

                if (ex is AggregateException aggException)
                {
                    ex = aggException.Flatten();
                }

                var sentryEvent = new SentryEvent(ex)
                {
                    Level = LoggingLevelMap[logEvent.Level],
                    Message = string.IsNullOrWhiteSpace(message) ? null : new SentryMessage(message),
                    Extra = extras
                };

                if (ex != null)
                {
                    foreach (DictionaryEntry data in ex.Data)
                    {
                        extras.Add(data.Key.ToString(), data.Value.ToString());
                    }
                }

                _client.Capture(sentryEvent);
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }
    }
}