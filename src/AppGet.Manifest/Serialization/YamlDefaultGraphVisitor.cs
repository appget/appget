using System;
using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace AppGet.Manifest.Serialization
{
    public sealed class YamlDefaultGraphVisitor : ChainedObjectGraphVisitor
    {
        public YamlDefaultGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
            : base(nextVisitor)
        {
        }

        private static object GetDefault(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        private static readonly IEqualityComparer<object> ObjectComparer = new JsonComparator<object>();

        public override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
        {
            return !ObjectComparer.Equals(value, GetDefault(value.Type)) && base.EnterMapping(key, value, context);
        }

        public override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
        {
            var defaultValueAttribute = key.GetCustomAttribute<DefaultValueAttribute>();
            var defaultValue = defaultValueAttribute != null ? defaultValueAttribute.Value : GetDefault(key.Type);

            if (ObjectComparer.Equals(value.Value, defaultValue))
            {
                return false;
            }

            return base.EnterMapping(key, value, context);
        }
    }
}