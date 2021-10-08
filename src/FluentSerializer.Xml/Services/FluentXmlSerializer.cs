using FluentSerializer.Xml.Configuration;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public sealed class FluentXmlSerializer : IXmlSerializer
    {
        public FluentXmlSerializer(List<XmlSerializerProfile> profiles)
        {
        }

        public TData Deserialize<TData>(string stringData)
        {
            throw new NotImplementedException();
        }

        public XDocument DeserializeToDocument(string dataObject)
        {
            throw new NotImplementedException();
        }

        public XElement DeserializeToElement(string dataObject)
        {
            throw new NotImplementedException();
        }

        public string Serialize(XObject dataObject)
        {
            throw new NotImplementedException();
        }

        public string Serialize<TData>(TData dataObject)
        {
            throw new NotImplementedException();
        }
    }
}
