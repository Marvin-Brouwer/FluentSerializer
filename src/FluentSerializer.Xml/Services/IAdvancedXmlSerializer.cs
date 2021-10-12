using System;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public interface IAdvancedXmlSerializer : IXmlSerializer
    {
        object? Deserialize(XElement element, Type modelType);
        XElement? SerializeToElement(object? model, Type modelType);
    }
}
