using FluentSerializer.Core;
using System;
using System.Xml.Linq;

namespace FluentSerializer.Xml
{
    public interface IXmlSerializer : ISerializer
    {
        string Serialize(XObject dataObject);
        XElement DeSerializeToElement(string dataObject);
        XDocument DeSerializeToDocument(string dataObject);
    }
}
