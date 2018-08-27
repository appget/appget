using System;

namespace AppGet.Infrastructure.Composition
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class HandlesAttribute : Attribute
    {
        private readonly Type _type;

        public HandlesAttribute(Type type)
        {
            _type = type;
        }

        public static Type GetValue(Type target)
        {
            var attr = (HandlesAttribute)GetCustomAttribute(target, typeof(HandlesAttribute));
            return attr?._type;
        }
    }
}