using FluentSerializer.Xml.DataNodes;
using System;

namespace FluentSerializer.Xml.Services
{
    public interface IAdvancedXmlSerializer : IXmlSerializer
    {
        object? Deserialize(IXmlElement element, Type modelType);
        IXmlElement? SerializeToElement(object? model, Type modelType);
    }
}
