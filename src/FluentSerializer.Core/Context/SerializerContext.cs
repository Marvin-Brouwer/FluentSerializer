using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System.Reflection;

namespace FluentSerializer.Core.Context
{
    public sealed class SerializerContext : ISerializerContext
    {
        public PropertyInfo Property { get; }
        public INamingStrategy NamingStrategy { get; }
        public ISerializer CurrentSerializer { get; }

        public SerializerContext(PropertyInfo property, INamingStrategy namingStrategy, ISerializer currentSerializer)
        {
            Property = property;
            NamingStrategy = namingStrategy;
            CurrentSerializer = currentSerializer;
        }
    }
}
