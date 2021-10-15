using System;
using System.Reflection;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    /// <summary>
    /// The RequestTypeValueConverter is used to reflect out the element name of the data passed.
    /// OpenAir requires this value to be matched exactly on the type attribute.
    /// </summary>
    internal class RequestTypeValueConverter : IXmlConverter<XAttribute>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Serialize;
        public bool CanConvert(Type targetType) => typeof(string) == targetType;
        public object Deserialize(XAttribute attributeToDeserialize, ISerializerContext context) => throw new NotSupportedException();

        public XAttribute? Serialize(object objectToSerialize, ISerializerContext context)
        {
            // We know this to be true because of RequestObject<TModel>
            var classType = context.ClassType.GetTypeInfo().GenericTypeArguments[0];
            var classNamingStrategy = context.FindNamingStrategy(classType);
            if (classNamingStrategy is null)
                throw new NotSupportedException($"Unable to find a NamingStrategy for '{classType.FullName}'");

            var elementTypeString = classNamingStrategy.SafeGetName(classType, context);
            var attributeName = context.NamingStrategy.SafeGetName(context.Property, context);

            return new XAttribute(attributeName, elementTypeString);
        }
    }
}