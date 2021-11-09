using System.Collections.Generic;

namespace FluentSerializer.Xml.DataNodes
{
    public interface IXmlElement : IXmlContainer<IXmlElement>
    {
        IXmlAttribute? GetChildAttribute(string name);

        IEnumerable<IXmlElement> GetChildElements(string? name = null);
        IXmlElement? GetChildElement(string name);

        string? GetTextValue();
    }
}