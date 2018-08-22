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
            const string DSN = "https://79eabeab1aa84c8db73ee2675c5bce7d@sentry.io/306545";

            _client = new RavenClient(new Dsn(DSN), new JsonPacketFactory(), new SentryRequestFactory(), new SentryUserFactory())
            {
                Compression = true,
                ErrorOnCapture = OnError,
                Release = BuildInfo.Version.ToString(),
                Environment = BuildInfo.IsProduction ? "Production" : "Dev",
            };

            var osInfo = new EnvInfo();

            _client.Tags.Add("culture", Thread.CurrentThread.CurrentCulture.Name);
            _client.Tags.Add("args", Environment.CommandLine);
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
                    _client.AddTrail(new Breadcrumb(logEvent.LoggerName, BreadcrumbType.Navigation)
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
                        extras.Add(data.Key.ToString(), data.Value?.ToString());
                    }
                }

                lock (_client)
                {
                    _client.Logger = logEvent.LoggerName;
                    var result = _client.Capture(sentryEvent);

                    _client.Logger = "root";

                    if (ex != null)
                    {
                        ex.Data[SENTRY_REPORTED] = true;
                        ex.Data["SENTRY_ID"] = result;
                    }
                }

            }
            catch (Exception e)
            {
                OnError(e);
            }
        }
    }
}