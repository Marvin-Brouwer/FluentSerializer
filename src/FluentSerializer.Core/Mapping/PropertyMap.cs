using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public abstract class PropertyMap<TDestination> : IPropertyMap 
        where TDestination : class
    {
        public SerializerDirection Direction { get; }
        public PropertyInfo Property { get; }
        public INamingStrategy NamingStrategy { get; }
        public IConverter? CustomConverter { get; }
        public Type DestinationType { get; }

        public PropertyMap(
            SerializerDirection direction,
            PropertyInfo property,
            INamingStrategy namingStrategy,
            IConverter? customConverter)
        {
            Direction = direction;
            Property = property;
            NamingStrategy = namingStrategy;
            CustomConverter = customConverter;
            DestinationType = typeof(TDestination);
        }
    }
}
