using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public abstract class PropertyMap : IPropertyMap 
    {
        public SerializerDirection Direction { get; }
        public PropertyInfo Property { get; }
        public Type ConcretePropertyType { get; }
        public INamingStrategy NamingStrategy { get; }
        public IConverter? CustomConverter { get; }
        public Type ContainerType { get; }

        protected PropertyMap(
            SerializerDirection direction,
            Type containerType,
            PropertyInfo property,
            INamingStrategy namingStrategy,
            IConverter? customConverter)
        {
            Direction = direction;
            Property = property;
            ConcretePropertyType = Nullable.GetUnderlyingType(property.PropertyType) 
                                   ?? property.PropertyType;
            NamingStrategy = namingStrategy;
            CustomConverter = customConverter;
            ContainerType = containerType;
        }
    }
}
