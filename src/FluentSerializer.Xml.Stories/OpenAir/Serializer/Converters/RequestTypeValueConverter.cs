using FluentSerializer.Xml.Profiles;
using System;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    /// <summary>
    /// The RequestTypeValueConverter is used to reflect out the element name of the data passed.
    /// OpenAir requires this value to be matched exactly on the type attribute.
    /// </summary>
    internal class RequestTypeValueConverter : ICustomAttributeConverter
    {
        public SerializerDirection Direction => SerializerDirection.Serialize;
        public bool CanConvert(PropertyInfo propertyInfo) => typeof(string) == propertyInfo.PropertyType;
        public object Deserialize(object? currentValue, XAttribute attributeToDeserialize, ISerializerContext context) => throw new NotSupportedException();

        public XAttribute? Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context)
        {
            // We know this to be true because of RequestObject<TModel>
            var classType = objectToSerialize.GetType().GetGenericArguments()[0];
            var wrapperElement = context.CurrentSerializer.SerializeToElement(Activator.CreateInstance(classType));
            var elementTypeString = wrapperElement.Name;

            var attributeName = context.NamingStrategy.GetName(context.Property);
            return new XAttribute(attributeName, elementTypeString);
        }
    }
}