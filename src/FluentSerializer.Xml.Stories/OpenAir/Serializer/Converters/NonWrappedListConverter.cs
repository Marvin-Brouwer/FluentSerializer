using FluentSerializer.Xml.Profiles;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    internal class NonWrappedListConverter : ICustomElementConverter
    {
        public SerializerDirection Direction => SerializerDirection.Both;

        public bool CanConvert(PropertyInfo propertyInfo) => typeof(IEnumerable<>).IsAssignableFrom(propertyInfo.PropertyType);

        public object Deserialize(object? currentValue, XElement elementToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            throw new System.NotImplementedException();
        }

        public XElement Serialize(XElement currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer)
        {
            throw new System.NotImplementedException();
        }
    }
}