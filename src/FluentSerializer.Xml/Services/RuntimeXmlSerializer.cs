using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Xml.Exceptions;
using System;
using System.Collections;
using System.Text;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Xml.Services
{
    public sealed class RuntimeXmlSerializer : IAdvancedXmlSerializer
    {
        private readonly XmlTypeSerializer _serializer;
        private readonly XmlTypeDeserializer _deserializer;
        private readonly ObjectPool<StringBuilder> _stringBuilderPool;

        public XmlSerializerConfiguration XmlConfiguration { get; }
        public SerializerConfiguration Configuration => XmlConfiguration;

        public RuntimeXmlSerializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings, XmlSerializerConfiguration configuration)
        {
            Guard.Against.Null(mappings, nameof(mappings));
            Guard.Against.Null(configuration, nameof(configuration));

            _serializer = new XmlTypeSerializer(mappings);
            _deserializer = new XmlTypeDeserializer(mappings);

            // todo make injectable
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool();

            XmlConfiguration = configuration;
        }

        public TModel? Deserialize<TModel>(IXmlElement element)
            where TModel : class, new()
        {
            if (typeof(IEnumerable).IsAssignableFrom(typeof(TModel))) throw new MalConfiguredRootNodeException(typeof(TModel));
            return _deserializer.DeserializeFromElement<TModel>(element, this);
        }

        public object? Deserialize(IXmlElement element, Type modelType)
        {
            return _deserializer.DeserializeFromElement(element, modelType,  this);
        }

        public TModel? Deserialize<TModel>(string stringData)
            where TModel : class, new()
        {
            var rootElement = XmlParser.Parse(stringData);
            return Deserialize<TModel>(rootElement);
        }

        public string Serialize<TModel>(TModel model)
            where TModel : class, new()
        {
            if (model is IEnumerable) throw new MalConfiguredRootNodeException(model.GetType());
            var document = SerializeToDocument(model);

            var stringBuilder = _stringBuilderPool.Get();

            using var writer = new ConfigurableStringWriter(stringBuilder, Configuration.Encoding);
            document.WriteTo(_stringBuilderPool, writer, Configuration.FormatOutput, Configuration.WriteNull);
            writer.Flush();

            var stringValue = stringBuilder.ToString();
            _stringBuilderPool.Return(stringBuilder);

            return stringValue;
        }

        public IXmlDocument SerializeToDocument<TModel>(TModel model)
        {
            var rootElement = SerializeToElement(model);

            return new XmlDocument(rootElement);
        }

        public IXmlElement? SerializeToElement<TModel>(TModel model)
        {
            if (model is null) return null;
            return _serializer.SerializeToElement(model, typeof(TModel), this);
        }

        public IXmlElement? SerializeToElement(object? model, Type modelType)
        {
            if (model is null) return null;
            return _serializer.SerializeToElement(model, modelType, this);
        }
    }
}
