using System;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public abstract class PropertyMap<TDestination> : IPropertyMap<TDestination> 
        where TDestination : class
    {
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
        public SerializerDirection Direction { get; }
        public PropertyInfo Property { get; }
        public INamingStrategy NamingStrategy { get; }
        public IConverter? CustomConverter { get; }
        public Type DestinationType { get; }
    }
}
