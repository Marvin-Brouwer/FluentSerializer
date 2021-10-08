using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public interface ICustomElementConverter
    {
        bool CanConvert(PropertyInfo propertyInfo);
        SerializerDirection Direction { get;}
        public XElement Serialize(XElement currentValue, object objectToSerialize, PropertyInfo property, IXmlSerializer currentSerializer);
        public object Deserialize(object? currentValue, XElement elementToDeserialize, PropertyInfo property, IXmlSerializer currentSerializer);
    }
}
