using System.Collections.Generic;

namespace FluentSerializer.Xml.DataNodes
{
    public interface IXmlElement : IXmlContainer<IXmlElement>
    {
        IReadOnlyList<IXmlAttribute> AttributeNodes { get; }
        IReadOnlyList<IXmlElement> ElementNodes { get; }
        IReadOnlyList<IXmlText> TextNodes { get; }
    }
}