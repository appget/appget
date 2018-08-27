using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using AppGet.Gui.Views;
using AppGet.Gui.Views.Installation;
using AppGet.Infrastructure.Composition;
using AppGet.Infrastructure.Logging;
using Caliburn.Micro;
using DryIoc;
using NLog;
using LogManager = NLog.LogManager;

namespace AppGet.Gui.Framework
{
    public class CaliburnBootstrapper : BootstrapperBase
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(CaliburnBootstrapper));
        private readonly Container _container;

        public CaliburnBootstrapper()
        {
            Initialize();
            LogConfigurator.EnableSentryTarget("https://79eabeab1aa84c8db73ee2675c5bce7d@sentry.io/306545");
            LogConfigurator.EnableFileTarget(LogLevel.Trace);

            _container = ContainerBuilder.Container;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                DisplayRootViewFor<ShellViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
                throw;
            }
        }

        protected override object GetInstance(Type service, string key)
        {
            if (_container.IsRegistered(service))
            {
                return _container.Resolve(service);
            }

            return base.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveMany(service);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.Exception);
            base.OnUnhandledException(sender, e);
        }
    }
}