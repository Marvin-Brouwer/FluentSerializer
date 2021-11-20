﻿using System;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    /// <summary>
    /// The RequestTypeValueConverter is used to reflect out the element name of the data passed.
    /// OpenAir requires this value to be matched exactly on the type attribute.
    /// </summary>
    public class RequestTypeValueConverter : IXmlConverter<IXmlAttribute>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Serialize;
        public bool CanConvert(Type targetType) => typeof(string) == targetType;
        public object Deserialize(IXmlAttribute attributeToDeserialize, ISerializerContext context) => throw new NotSupportedException();

        public IXmlAttribute? Serialize(object objectToSerialize, ISerializerContext context)
        {
            // We know this to be true because of RequestObject<TModel>
            var classType = context.ClassType.GetTypeInfo().GenericTypeArguments[0];
            var classNamingStrategy = context.FindNamingStrategy(classType);
            if (classNamingStrategy is null)
                throw new NotSupportedException($"Unable to find a NamingStrategy for '{classType.FullName}'");

            var elementTypeString = classNamingStrategy.SafeGetName(classType, context);
            var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);

            return Attribute(attributeName, elementTypeString);
        }
    }
}