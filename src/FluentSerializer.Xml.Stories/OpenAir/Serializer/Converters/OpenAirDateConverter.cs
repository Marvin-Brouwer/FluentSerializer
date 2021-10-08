using FluentSerializer.Xml.Profiles;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    internal class OpenAirDateConverter : ICustomAttributeConverter, ICustomElementConverter
    {
        private readonly SerializerDirection _direction = SerializerDirection.Both;
        SerializerDirection ICustomAttributeConverter.Direction => _direction;
        SerializerDirection ICustomElementConverter.Direction => _direction;

        private bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);
        bool ICustomAttributeConverter.CanConvert(PropertyInfo property) => CanConvert(property);
        bool ICustomElementConverter.CanConvert(PropertyInfo property) => CanConvert(property);

        object ICustomAttributeConverter.Deserialize(object? currentValue, XAttribute attributeToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            throw new System.NotImplementedException();
        }

        object ICustomElementConverter.Deserialize(object? currentValue, XElement elementToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            throw new System.NotImplementedException();
        }

        XAttribute ICustomAttributeConverter.Serialize(XAttribute? currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            throw new System.NotImplementedException();
        }

        XElement ICustomElementConverter.Serialize(XElement? currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            throw new System.NotImplementedException();
        }
    }
}