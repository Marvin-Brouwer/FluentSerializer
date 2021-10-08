using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public interface ICustomAttributeConverter
    {
        bool CanConvert(PropertyInfo propertyInfo);
        SerializerDirection Direction { get;}
        public XAttribute? Serialize(XAttribute? currentValue, object objectToSerialize, ISerializerContext context);
        public object? Deserialize(object? currentValue, XAttribute attributeToDeserialize, ISerializerContext context);
    }
}
