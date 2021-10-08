using FluentSerializer.Xml.Profiles;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public class NonWrappedListConverter : ICustomElementConverter
    {
        public SerializerDirection Direction => SerializerDirection.Both;

        public bool CanConvert(PropertyInfo propertyInfo) => typeof(IEnumerable<>).IsAssignableFrom(propertyInfo.PropertyType);

        public object? Deserialize(object? currentValue, XElement elementToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        public XElement? Serialize(XElement? currentValue, object objectToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}