using FluentSerializer.Core.Configuration;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.SerializerException
{
    [Serializable]
    public sealed class ConverterNotSupportedException : SerializerException
    {
        public Type TargetType { get; }
        public PropertyInfo Property { get; }
        public Type ConverterType { get; }
        public Type ContainerType { get; }
        public SerializerDirection Direction { get; }

        public ConverterNotSupportedException(IPropertyMap property, Type converterType, Type containerType, SerializerDirection direction) : base(
            $"The converter of type '{converterType}' selected for '{property.ContainerType.FullName ?? "<dynamic>"}.{property.Property.Name}' cannot convert '{property.ConcretePropertyType.FullName}' \n" +
            "Make sure you've selected a converter that supports this conversion.")
        {
            TargetType = property.ConcretePropertyType;
            Property = property.Property;
            ContainerType = containerType;
            ConverterType = converterType;
            Direction = direction;
        }

        #region Serializable
        private ConverterNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            TargetType = (Type)info.GetValue(nameof(TargetType), typeof(Type))!;
            Property = (PropertyInfo)info.GetValue(nameof(Property), typeof(PropertyInfo))!;
            ConverterType = (Type)info.GetValue(nameof(ConverterType), typeof(Type))!;
            ContainerType = (Type)info.GetValue(nameof(ContainerType), typeof(Type))!;
            Direction = (SerializerDirection)info.GetValue(nameof(Direction), typeof(SerializerDirection))!;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(TargetType), TargetType);
            info.AddValue(nameof(Property), Property);
            info.AddValue(nameof(ConverterType), ConverterType);
            info.AddValue(nameof(ContainerType), ContainerType);
            info.AddValue(nameof(Direction), Direction);

            base.GetObjectData(info, context);
        }
        #endregion
    }
}
