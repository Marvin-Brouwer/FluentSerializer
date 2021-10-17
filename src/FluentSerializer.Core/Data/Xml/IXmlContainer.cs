using System.Collections.Generic;

namespace FluentSerializer.Core.Data.Xml
{
    public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode
    {
        IReadOnlyList<XmlAttribute> AttributeNodes { get; }
        IReadOnlyList<XmlElement> ElementNodes { get; }
        IReadOnlyList<XmlText> TextNodes { get; }
    }
}
