using FluentSerializer.Core.Configuration;
using FluentSerializer.Xml.Mapping;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public sealed class FluentXmlSerializer : IXmlSerializer, IAdvancedXmlSerializer
    {
        private static readonly XDeclaration DefaultXmlDeclaration = new XDeclaration("v1.0", Encoding.UTF8.WebName, "true");

        private readonly XmlTypeSerializer _serializer;
        private readonly XmlTypeDeserializer _deserializer;

        public SerializerConfiguration Configuration { get; }

        public FluentXmlSerializer(ILookup<Type, XmlClassMap> mappings, SerializerConfiguration? configuration = null)
        {
            _serializer = new XmlTypeSerializer(mappings);
            _deserializer = new XmlTypeDeserializer(mappings);

            Configuration = configuration ?? ConfigurationConstants.GetDefaultXmlConfiguration();
        }

        public TModel? Deserialize<TModel>(XElement dataObject)
            where TModel : class, new()
        {
            return _deserializer.DeserializeFromObject<TModel>(dataObject, this);
        }
        public object? Deserialize(XElement dataObject, Type modelType)
        {
            return _deserializer.DeserializeFromObject(modelType, dataObject, this);
        }

        public TModel? Deserialize<TModel>(string stringData)
            where TModel : class, new()
        {
            // todo see what happens if an element without declaration is passed
            var xDocument = XDocument.Parse(stringData);
            return Deserialize<TModel>(xDocument.Root);
        }

        public string Serialize<TModel>(TModel model)
        {
            return SerializeToDocument(model).ToString();
        }

        public XDocument SerializeToDocument<TModel>(TModel model, XDeclaration? declaration = null)
        {
            var rootElement = SerializeToElement(model);
            return new XDocument(
                declaration ?? DefaultXmlDeclaration,
                rootElement
            );
        }

        public XElement? SerializeToElement<TModel>(TModel model)
        {
            if (model is null) return null;
            return _serializer.SerializeToElement(model, typeof(TModel), this);
        }
        public XElement? SerializeToElement(object model, Type modelType)
        {
            if (model is null) return null;
            return _serializer.SerializeToElement(model, modelType, this);
        }
    }
}
