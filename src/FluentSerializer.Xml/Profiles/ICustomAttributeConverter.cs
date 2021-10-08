using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public interface ICustomAttributeConverter
    {
        bool CanConvert(PropertyInfo propertyInfo);
        SerializerDirection Direction { get;}
        public XAttribute Serialize(XAttribute currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer);
        public object Deserialize(object? currentValue, XAttribute attributeToDeserialize, PropertyInfo property, IXmlSerializer currentSerializer);
    }
}
