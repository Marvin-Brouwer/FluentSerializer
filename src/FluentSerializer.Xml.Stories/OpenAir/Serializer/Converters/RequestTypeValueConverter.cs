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
        public bool CanConvert(PropertyInfo propertyInfo) => typeof(string) == propertyInfo.PropertyType;
        public object Deserialize(XAttribute attributeToDeserialize, ISerializerContext context) => throw new NotSupportedException();

        public XAttribute? Serialize(object objectToSerialize, ISerializerContext context)
        {
            // We know this to be true because of RequestObject<TModel>
            var classType = objectToSerialize.GetType().GetGenericArguments()[0];
            var wrapperElement = ((IXmlSerializer)context.CurrentSerializer).SerializeToElement(Activator.CreateInstance(classType));
            var elementTypeString = wrapperElement?.Name;
            // todo check name for null and throw

            var attributeName = context.NamingStrategy.GetName(context.Property);
            return new XAttribute(attributeName, elementTypeString);
        }
    }
}