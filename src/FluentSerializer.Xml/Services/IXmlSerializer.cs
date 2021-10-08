using FluentSerializer.Core;
using System.Xml.Linq;

namespace FluentSerializer.Xml
{
    public interface IXmlSerializer : ISerializer
    {
        string Serialize(XObject dataObject);
        XElement DeserializeToElement(string dataObject);
        XDocument DeserializeToDocument(string dataObject);
    }
}
