using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics.CodeAnalysis;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Services
{
	public sealed class RuntimeJsonSerializer : IAdvancedJsonSerializer
    {
        private readonly JsonTypeSerializer _serializer;
        private readonly JsonTypeDeserializer _deserializer;
        private readonly ObjectPool<ITextWriter> _stringBuilderPool;

		public JsonSerializerConfiguration JsonConfiguration { get; }
		public SerializerConfiguration Configuration => JsonConfiguration;

		public RuntimeJsonSerializer(
			IScanList<(Type type, SerializerDirection direction), IClassMap> mappings, 
			JsonSerializerConfiguration configuration,
			ObjectPoolProvider objectPoolProvider)
		{
			Guard.Against.Null(mappings, nameof(mappings));
			Guard.Against.Null(configuration, nameof(configuration));

            _serializer = new JsonTypeSerializer(mappings);
            _deserializer = new JsonTypeDeserializer(mappings);
            _stringBuilderPool = objectPoolProvider.CreateStringFastPool(configuration.Encoding, configuration.NewLine);

			JsonConfiguration = configuration;
		}

		public TModel? Deserialize<TModel>([MaybeNull, AllowNull] IJsonContainer? element) where TModel : new()
		{
			if (element is null) return default;

			return _deserializer.DeserializeFromNode<TModel>(element, this);
		}

		public object? Deserialize([MaybeNull, AllowNull] IJsonContainer? element, Type modelType)
		{
			if (element is null) return default;

			return _deserializer.DeserializeFromNode(element, modelType,  this);
		}
		public TModel? Deserialize<TModel>([MaybeNull, AllowNull] string? stringData) where TModel : new()
		{
			if (string.IsNullOrEmpty(stringData)) return default;

			var jObject = JsonParser.Parse(stringData);
			return Deserialize<TModel>(jObject);
		}

		public string Serialize<TModel>([MaybeNull, AllowNull] TModel? model) where TModel : new()
		{
			var container = SerializeToContainer(model);
			if (container is null) return string.Empty;

			var stringValue =  container.WriteTo(_stringBuilderPool, Configuration.FormatOutput, Configuration.WriteNull);

			return stringValue;
		}

		public IJsonContainer? SerializeToContainer<TModel>(TModel? model)
		{
			if (model is null) return default;
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
