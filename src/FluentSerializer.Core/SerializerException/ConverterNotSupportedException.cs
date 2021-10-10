using FluentSerializer.Core.Configuration;
using System;
using System.Reflection;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class ConverterNotSupportedException : SerializerException
    {
        public Type TargetType { get; }
        public PropertyInfo Property { get; }
        public Type ConverterType { get; }
        public Type ContainerType { get; }
        public SerializerDirection Direction { get; }

        public ConverterNotSupportedException(PropertyInfo property, Type converterType, Type containerType, SerializerDirection direction) : base(
            $"The converter of type '{converterType}' selected for '{property.DeclaringType?.FullName ?? "<dynamic>"}.{property.Name}' cannot convert '{property.PropertyType.FullName}' \n" +
            "Make sure you've selected a converter that supports this conversion.")
        {
            TargetType = property.PropertyType;
            Property = property;
            ContainerType = containerType;
            ConverterType = converterType;
            Direction = direction;
        }
    }
}
