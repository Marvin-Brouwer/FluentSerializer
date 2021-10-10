using System;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public interface IAdvancedXmlSerializer : IXmlSerializer
    {
        object? Deserialize(XElement item, Type propertyType);
        XElement? SerializeToElement(object collectionItem, Type propertyType);
    }
}
