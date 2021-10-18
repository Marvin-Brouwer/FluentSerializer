using FluentSerializer.Core.DataNodes;
using System.Collections.Generic;

namespace FluentSerializer.Xml.DataNodes
{
    public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode
    {
        IReadOnlyList<XmlAttribute> AttributeNodes { get; }
        IReadOnlyList<XmlElement> ElementNodes { get; }
        IReadOnlyList<XmlText> TextNodes { get; }
    }
}
