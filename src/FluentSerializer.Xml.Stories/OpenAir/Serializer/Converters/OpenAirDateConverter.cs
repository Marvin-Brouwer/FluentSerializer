using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Profiles;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters
{
    public class OpenAirDateConverter : IConverter<XAttribute>, IConverter<XElement>
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