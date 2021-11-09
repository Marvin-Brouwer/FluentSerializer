using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using System;
using System.Text;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Core.Services;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;
using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Json.Services
{
    public sealed class RuntimeJsonSerializer : IAdvancedJsonSerializer
    {
        private readonly JsonTypeSerializer _serializer;
        private readonly JsonTypeDeserializer _deserializer;
        private readonly ObjectPool<StringBuilder> _stringBuilderPool;

        public JsonSerializerConfiguration JsonConfiguration { get; }
        public SerializerConfiguration Configuration => JsonConfiguration;

        public RuntimeJsonSerializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings, JsonSerializerConfiguration configuration)
        {
            Guard.Against.Null(mappings, nameof(mappings));
            Guard.Against.Null(configuration, nameof(configuration));

            _serializer = new JsonTypeSerializer(mappings);
            _deserializer = new JsonTypeDeserializer(mappings);
            
            // todo make injectable
            var objectPoolProvider = new DefaultObjectPoolProvider();
            _stringBuilderPool = objectPoolProvider.CreateStringBuilderPool();

            JsonConfiguration = configuration;
        }

        public TModel? Deserialize<TModel>(IJsonContainer element) where TModel : class, new()
        {
            return _deserializer.DeserializeFromNode<TModel>(element, this);
        }

        public object? Deserialize(IJsonContainer element, Type modelType)
        {
            return _deserializer.DeserializeFromNode(element, modelType,  this);
        }

        public TModel? Deserialize<TModel>(string stringData) where TModel : class, new()
        {
            var jObject = JsonParser.Parse(stringData);
            return Deserialize<TModel>(jObject);
        }

        public string Serialize<TModel>(TModel model) where TModel : class, new()
        {
            var container = SerializeToContainer(model);
            if (container is null) return string.Empty;

            var stringBuilder = _stringBuilderPool.Get();

            using var writer = new ConfigurableStringWriter(stringBuilder, Configuration.Encoding);
            // todo configuration.writenull
            container.WriteTo(_stringBuilderPool, writer, Configuration.FormatOutput);
            writer.Flush();

            var stringValue = stringBuilder.ToString();
            _stringBuilderPool.Return(stringBuilder);

            return stringValue;
        }

        public IJsonContainer? SerializeToContainer<TModel>(TModel model)
        {
            if (model is null) return null;
            var token = _serializer.SerializeToNode(model, typeof(TModel), this);
            if (token is null) return Object();
            if (token is IJsonContainer container) return container;

            throw new NotSupportedException();
        }

        public TContainer? SerializeToContainer<TContainer>(object? model, Type modelType) where TContainer : IJsonContainer
        {
            if (model is null) return default;
            var token = _serializer.SerializeToNode(model, modelType, this);
            if (token is null) return default;
            if (token is TContainer container) return container;

            throw new NotSupportedException();
        }
    }
}
