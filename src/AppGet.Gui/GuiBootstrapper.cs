using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AppGet.Commands;
using AppGet.Commands.Install;
using AppGet.Infrastructure.Composition;
using Caliburn.Micro;

namespace AppGet.Gui
{
    public class GuiBootstrapper : BootstrapperBase
    {
        private readonly TinyIoCContainer _container;

        public GuiBootstrapper()
        {
            Initialize();
            _container = ContainerBuilder.Build();

            var cc = _container.Resolve<InstallOptions>();

            var parser = _container.Resolve<IParseOptions>();
            var args = Environment.GetCommandLineArgs();
            var option = parser.Parse(args.Last());

            _container.Register((InstallOptions)option);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
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
    }
}