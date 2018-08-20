using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using AppGet.Gui.Views;
using AppGet.Gui.Views.Installation;
using AppGet.Infrastructure.Composition;
using Caliburn.Micro;
using NLog;
using LogManager = NLog.LogManager;

namespace AppGet.Gui.Framework
{
    public class CaliburnBootstrapper : BootstrapperBase
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(CaliburnBootstrapper));
        private readonly TinyIoCContainer _container;

        public CaliburnBootstrapper()
        {
            Initialize();
            _container = ContainerBuilder.Container;
            _container.RegisterMultiple<ICommandViewModel>(new[] { typeof(InstallCommandViewModel) });

            _container.Register<AppSession>().AsSingleton();
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
            if (_container.CanResolve(service))
            {
                return _container.Resolve(service);
            }

            return base.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveAll(service);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.Exception);
            base.OnUnhandledException(sender, e);
        }
    }
}