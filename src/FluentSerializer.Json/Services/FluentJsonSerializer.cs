using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using System;
using System.IO;
using System.Text;
using FluentSerializer.Json.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FluentSerializer.Core.Services;

namespace FluentSerializer.Json.Services
{
    public sealed class FluentJsonSerializer : IAdvancedJsonSerializer
    {
        private readonly JsonTypeSerializer _serializer;
        private readonly JsonTypeDeserializer _deserializer;

        public JsonSerializerConfiguration JsonConfiguration { get; }
        public SerializerConfiguration Configuration => JsonConfiguration;

        public JsonSerializerConfiguration XmlConfiguration => throw new NotImplementedException();

        public FluentJsonSerializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings, JsonSerializerConfiguration configuration)
        {
            Guard.Against.Null(mappings, nameof(mappings));
            Guard.Against.Null(configuration, nameof(configuration));

            _serializer = new JsonTypeSerializer(mappings);
            _deserializer = new JsonTypeDeserializer(mappings);

            JsonConfiguration = configuration;
        }
        

        //public TModel? Deserialize<TModel>(XElement element)
        //    where TModel : class, new()
        //{
        //    if (typeof(IEnumerable).IsAssignableFrom(typeof(TModel))) throw new MalConfiguredRootNodeException(typeof(TModel));
        //    return _deserializer.DeserializeFromObject<TModel>(element, this);
        //}
        public TModel? Deserialize<TModel>(JContainer element) where TModel : class, new()
        {
            return _deserializer.DeserializeFromToken<TModel>(element, this);
        }

        //public object? Deserialize(XElement element, Type modelType)
        //{
        //    return _deserializer.DeserializeFromObject(element, modelType,  this);
        //}
        public object? Deserialize(JContainer element, Type modelType)
        {
            return _deserializer.DeserializeFromToken(element, modelType,  this);
        }

        //public TModel? Deserialize<TModel>(string stringData)
        //    where TModel : class, new()
        //{
        //    var xDocument = XDocument.Parse(stringData);
        //    return Deserialize<TModel>(xDocument.Root!);
        //}
        public TModel? Deserialize<TModel>(string stringData) where TModel : class, new()
        {
            var jObject = JObject.Parse(stringData);
            return Deserialize<TModel>(jObject);
        }

        //public string Serialize<TModel>(TModel model)
        //    where TModel : class, new()
        //{
        //    if (model is IEnumerable) throw new MalConfiguredRootNodeException(model.GetType());
        //    var xDocument = SerializeToDocument(model);

        //    var builder = new StringBuilder();
        //    using var writer = new ConfigurableStringWriter(builder, Configuration.Encoding);

        //    xDocument.Save(writer);

        //    return builder.ToString();
        //}
        public string Serialize<TModel>(TModel model) where TModel : class, new()
        {
            var container = SerializeToContainer(model);
            if (container is null) return string.Empty;

            var builder = new StringBuilder();
            using var writer = new ConfigurableStringWriter(builder, Configuration.Encoding);
            using var jWriter = new JsonTextWriter(writer);

            container.WriteTo(jWriter);

            return builder.ToString();
        }

        //public XDocument SerializeToDocument<TModel>(TModel model)
        //{
        //    var rootElement = SerializeToElement(model);

        //    var document = new XDocument(rootElement);

        //    return document;
        //}


        //public XElement? SerializeToElement<TModel>(TModel model)
        //{
        //    if (model is null) return null;
        //    return _serializer.SerializeToElement(model, typeof(TModel), this);
        //}
        public JContainer? SerializeToContainer<TModel>(TModel model)
        {
            if (model is null) return new JObject(JValue.CreateNull());
            var token = _serializer.SerializeToToken(model, typeof(TModel), this);
            if (token is null) return null;
            if (token is JContainer container) return container;

            throw new NotSupportedException();
        }

        //public XElement? SerializeToElement(object? model, Type modelType)
        //{
        //    if (model is null) return null;
        //    return _serializer.SerializeToElement(model, modelType, this);
        //}
        public TContainer? SerializeToContainer<TContainer>(object? model, Type modelType) where TContainer : JContainer
        {
            if (model is null) return null;
            var token = _serializer.SerializeToToken(model, modelType, this);
            if (token is null) return null;
            if (token is TContainer container) return container;

            throw new NotSupportedException();
        }
    }
}
