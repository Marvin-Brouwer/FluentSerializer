using FluentSerializer.Xml.Profiles;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public sealed class FluentXmlSerializer : IXmlSerializer
    {
        private static readonly XDeclaration DefaultXmlDeclaration = new XDeclaration("v1.0", Encoding.UTF8.WebName, "true");

        private readonly XmlTypeSerializer _serializer;

        public FluentXmlSerializer(ILookup<Type, XmlClassMap> mappings)
        {
            _serializer = new XmlTypeSerializer(mappings);
        }

        public TModel Deserialize<TModel>(XObject dataObject)
        {
            throw new NotImplementedException();
        }

        public TData Deserialize<TData>(string stringData)
        {
            throw new NotImplementedException();
        }

        public string Serialize<TData>(TData dataObject)
        {
            return SerializeToDocument(dataObject).ToString();
        }

        public XDocument SerializeToDocument<TModel>(TModel dataObject, XDeclaration? declaration = null)
        {
            var rootElement = SerializeToElement(dataObject);
            return new XDocument(
                declaration ?? DefaultXmlDeclaration,
                rootElement
            );
        }

        public XElement? SerializeToElement<TModel>(TModel dataObject)
        {
           return _serializer.SerializeToElement(dataObject, this);
        }
    }
}
