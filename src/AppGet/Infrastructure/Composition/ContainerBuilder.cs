using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AppGet.Commands;
using DryIoc;
using NLog;

namespace AppGet.Infrastructure.Composition
{
    public static class ContainerBuilder
    {
        private static readonly List<Type> AssemblyTypes;

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

            Container.Register(Made.Of(() => LogManager.GetLogger(Arg.Index<string>(0)), request => request.Parent.ImplementationType.Name));

            bool ShouldRegisterAs(Type type)
            {
                if (type.IsInterface) return true;
                if (type.IsAbstract) return true;

                return false;
            }

            Container.RegisterMany(ass, ShouldRegisterAs, made: made);

            RegisterHandlers<ICommandHandler>();
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

        public static Container Container { get; }
    }
}
