using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Services;
using System;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public sealed class PropertyMap : IPropertyMap 
    {
        public SerializerDirection Direction { get; }
        public PropertyInfo Property { get; }
        public Type ConcretePropertyType { get; }
        public INamingStrategy NamingStrategy { get; }
        public IConverter? CustomConverter { get; }
        public Type ContainerType { get; }

        public PropertyMap(
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

        public IConverter<TDataContainer>? GetConverter<TDataContainer>(
            SerializerDirection direction, ISerializer currentSerializer)
            where TDataContainer : class
        {
            Guard.Against.Null(direction, nameof(direction));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            var converter = CustomConverter ?? currentSerializer.Configuration.DefaultConverters
                .Where(converter => converter is IConverter<TDataContainer>)
                .Where(converter => converter.Direction == SerializerDirection.Both || converter.Direction == direction)
                .FirstOrDefault(converter => converter.CanConvert(ConcretePropertyType));
            if (converter is null) return null;

            if (!converter.CanConvert(ConcretePropertyType))
                throw new ConverterNotSupportedException(Property, converter.GetType(), typeof(TDataContainer), direction);
            if (converter is IConverter<TDataContainer> specificConverter)
                return specificConverter;

            throw new ConverterNotSupportedException(Property, converter.GetType(), typeof(TDataContainer), direction);
        }
    }
}
