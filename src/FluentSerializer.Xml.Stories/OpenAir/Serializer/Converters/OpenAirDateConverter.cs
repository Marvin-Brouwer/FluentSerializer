using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters
{
    public class OpenAirDateConverter : IConverter<XAttribute>, IConverter<XElement>
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(PropertyInfo property) => typeof(bool).IsAssignableFrom(property.PropertyType);

        object IConverter<XAttribute>.Deserialize(XAttribute attributeToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        object IConverter<XElement>.Deserialize(XElement elementToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        XAttribute IConverter<XAttribute>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        XElement IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}