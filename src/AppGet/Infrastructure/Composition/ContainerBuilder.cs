using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AppGet.Commands;
using AppGet.Infrastructure.Eventing;
using AppGet.Infrastructure.Eventing.Events;
using AppGet.Installers.InstallerWhisperer;
using AppGet.Installers.UninstallerWhisperer;
using DryIoc;
using NLog;

namespace AppGet.Infrastructure.Composition
{
    public static class ContainerBuilder
    {
        public static Container Container { get; }


        public static readonly List<Type> AssemblyTypes;

        static ContainerBuilder()
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "appget.*", SearchOption.TopDirectoryOnly).Where(c => c.EndsWith(".exe") || c.EndsWith(".dll"));

            var ass = files.Select(Assembly.LoadFile).ToList();

            AssemblyTypes = ass.SelectMany(c => c.ExportedTypes
                .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsEnum && t.Namespace != null && t.Namespace.StartsWith("AppGet.")))
                .ToList();

            var rules = Rules.Default
                .WithAutoConcreteTypeResolution()
                .WithDefaultReuse(Reuse.Singleton)
                .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.AppendNotKeyed);


            var made = FactoryMethod.Constructor(mostResolvable: true);
            Container = new Container(rules);

            Container.RegisterMany(ass, ShouldRegisterAs, made: made);

            RegisterLoggerFactory();

            RegisterTransienttInstallers(made);

            RegisterHandlers<ICommandHandler>();

            Container.Resolve<IHub>().Publish(new ApplicationStartedEvent());
        }

        private static void RegisterLoggerFactory()
        {
            Container.Register(Made.Of(() => LogManager.GetLogger(Arg.Index<string>(0)), request => request.Parent.ImplementationType.Name), Reuse.Transient);
        }

        private static void RegisterTransienttInstallers(FactoryMethodSelector made)
        {
            Container.Unregister<InstallerBase>();
            Container.RegisterMany(AssemblyTypes.Where(c => c.ImplementsServiceType<InstallerBase>()), serviceTypeCondition: ShouldRegisterAs, made: made,
                reuse: Reuse.Transient);

            Container.Unregister<UninstallerBase>();
            Container.RegisterMany(AssemblyTypes.Where(c => c.ImplementsServiceType<UninstallerBase>()), serviceTypeCondition: ShouldRegisterAs, made: made,
                reuse: Reuse.Transient);
        }

        private static bool ShouldRegisterAs(Type type)
        {
            if (type.Namespace != null && !type.Namespace.StartsWith("AppGet.")) return false;
            if (type.IsInterface) return true;
            if (type.IsAbstract) return true;

            return false;
        }

        private static void RegisterHandlers<THandler>()
        {
            Container.Unregister<THandler>();
            var made = FactoryMethod.Constructor(mostResolvable: true);


            foreach (var handler in AssemblyTypes.Where(c => c.ImplementsServiceType<THandler>()))
            {
                var handles = HandlesAttribute.GetValue(handler);
                if (handles != null)
                {
                    Container.Register(typeof(THandler),
                        handler,
                        made: made,
                        serviceKey: handles);
                }
            }
        }

    }
}
