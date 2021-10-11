using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Xml.Exceptions;
using System;
using System.Collections;
using System.Text;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public sealed class FluentXmlSerializer : IXmlSerializer, IAdvancedXmlSerializer
    {
        private readonly XmlTypeSerializer _serializer;
        private readonly XmlTypeDeserializer _deserializer;

        public SerializerConfiguration Configuration { get; }

        public FluentXmlSerializer(IScanList<Type, IClassMap> mappings, SerializerConfiguration configuration)
        {
            Guard.Against.Null(mappings, nameof(mappings));
            Guard.Against.Null(configuration, nameof(configuration));

            _serializer = new XmlTypeSerializer(mappings);
            _deserializer = new XmlTypeDeserializer(mappings);

            Configuration = configuration;
        }

        public TModel? Deserialize<TModel>(XElement dataObject)
            where TModel : class, new()
        {
            if (typeof(IEnumerable).IsAssignableFrom(typeof(TModel))) throw new MissingRootNodeException(typeof(TModel));
            return _deserializer.DeserializeFromObject<TModel>(dataObject, this);
        }
        public object? Deserialize(XElement dataObject, Type modelType)
        {
            return _deserializer.DeserializeFromObject(dataObject,modelType,  this);
        }

        public TModel? Deserialize<TModel>(string stringData)
            where TModel : class, new()
        {
            // todo see what happens if an element without declaration is passed
            var xDocument = XDocument.Parse(stringData);
            return Deserialize<TModel>(xDocument.Root);
        }

        public string Serialize<TModel>(TModel model)
            where TModel : class, new()
        {
            if (typeof(IEnumerable).IsAssignableFrom(model.GetType())) throw new MissingRootNodeException(model.GetType());
            var xDocument = SerializeToDocument(model);

            var builder = new StringBuilder();
            using var writer = new ConfigurableStringWriter(builder, Configuration.Encoding);

            xDocument.Save(writer);

            return builder.ToString();
        }

        public XDocument SerializeToDocument<TModel>(TModel model)
        {
            var rootElement = SerializeToElement(model);

            var document = new XDocument(rootElement);

            return document;
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
