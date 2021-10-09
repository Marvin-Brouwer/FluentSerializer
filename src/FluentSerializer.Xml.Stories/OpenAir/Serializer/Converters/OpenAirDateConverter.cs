using FluentSerializer.Xml.Profiles;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public class OpenAirDateConverter : ICustomAttributeConverter, ICustomElementConverter
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);

        object IConverter<XAttribute>.Deserialize(object? currentValue, XAttribute attributeToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        object IConverter<XElement>.Deserialize(object? currentValue, XElement elementToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        XAttribute IConverter<XAttribute>.Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        XElement IConverter<XElement>.Serialize(XElement? currentValue, object objectToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}