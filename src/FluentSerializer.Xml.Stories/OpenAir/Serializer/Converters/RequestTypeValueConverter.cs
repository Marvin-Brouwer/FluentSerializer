using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Services;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters
{
    /// <summary>
    /// The RequestTypeValueConverter is used to reflect out the element name of the data passed.
    /// OpenAir requires this value to be matched exactly on the type attribute.
    /// </summary>
    internal class RequestTypeValueConverter : IConverter<XAttribute>
    {
        public SerializerDirection Direction => SerializerDirection.Serialize;
        public bool CanConvert(Type targetType) => typeof(string) == targetType;
        public object Deserialize(XAttribute attributeToDeserialize, ISerializerContext context) => throw new NotSupportedException();

        public XAttribute? Serialize(object objectToSerialize, ISerializerContext context)
        {
            // We know this to be true because of RequestObject<TModel>
            var classType = context.ClassType.GetTypeInfo().GenericTypeArguments[0];

            //var listType = typeof(List<>).GetGenericTypeDefinition().MakeGenericType(classType);
            var classInstance = Activator.CreateInstance(classType)!;
            //var classInstance = Activator.CreateInstance(listType)!;
            var wrapperElement = ((IAdvancedXmlSerializer)context.CurrentSerializer).SerializeToElement(classInstance, classType);
            var elementTypeString = wrapperElement?.Name.ToString();
            // todo check name for null and throw

            var attributeName = context.NamingStrategy.GetName(context.Property);
            return new XAttribute(attributeName, elementTypeString);
        }
    }
}