using FluentSerializer.Core.Configuration;
using System;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class ConverterNotFoundException : SerializerException
    {
        public Type TargetType { get; }
        public Type ContainerType { get; }
        public SerializerDirection Direction { get; }

        public ConverterNotFoundException(Type targetType, Type containerType, SerializerDirection direction) : base(
            $"No IConverter found for '{targetType.FullName}' \n" +
            "Make sure you've registered or selected a converter that supports this conversion.")
        {
            TargetType = targetType;
            ContainerType = containerType;
            Direction = direction;
        }
    }
}
