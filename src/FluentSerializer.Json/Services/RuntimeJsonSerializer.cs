using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Json.Services;

/// <inheritdoc />
public sealed class RuntimeJsonSerializer : IAdvancedJsonSerializer
{
	private readonly JsonTypeSerializer _serializer;
	private readonly JsonTypeDeserializer _deserializer;
	private readonly ObjectPool<ITextWriter> _stringBuilderPool;

	/// <inheritdoc />
	public JsonSerializerConfiguration JsonConfiguration { get; }
	/// <inheritdoc />
	public SerializerConfiguration Configuration => JsonConfiguration;

	/// <inheritdoc cref="RuntimeJsonSerializer" />
	public RuntimeJsonSerializer(
		JsonSerializerConfiguration configuration,
		ObjectPoolProvider objectPoolProvider,
		IReadOnlyCollection<IClassMap> mappings)
	{
		Guard.Against.Null(mappings, nameof(mappings));
		Guard.Against.Null(configuration, nameof(configuration));

		_serializer = new JsonTypeSerializer(in mappings);
		_deserializer = new JsonTypeDeserializer(in mappings);
		_stringBuilderPool = objectPoolProvider.CreateStringBuilderPool(configuration);

		JsonConfiguration = configuration;
	}

	/// <inheritdoc />
	public TModel? Deserialize<TModel>([MaybeNull, AllowNull] in IJsonContainer? element) where TModel : new()
	{
		if (element is null) return default;

		var coreContext = new SerializerCoreContext<IJsonNode>(this)
		{
			RootNode = element
		};
		return _deserializer.DeserializeFromNode<TModel>(element, coreContext);
	}

	/// <inheritdoc />
	public object? Deserialize([MaybeNull, AllowNull] in IJsonContainer? element, in Type modelType, in ISerializerCoreContext<IJsonNode> context)
	{
		if (element is null) return default;
		
		return _deserializer.DeserializeFromNode(element, in modelType, in context);
	}

	/// <inheritdoc />
	public TModel? Deserialize<TModel>([MaybeNull, AllowNull] in string? stringData) where TModel : new()
	{
		if (string.IsNullOrEmpty(stringData)) return default;

		var jObject = JsonParser.Parse(in stringData);
		return Deserialize<TModel>(jObject);
	}

	/// <inheritdoc />
	public string Serialize<TModel>([MaybeNull, AllowNull] in TModel? model) where TModel : new()
	{
		var container = SerializeToContainer(in model);
		if (container is null) return string.Empty;

		var stringValue =  container.WriteTo(in _stringBuilderPool, Configuration.FormatOutput, Configuration.WriteNull);

		return stringValue;
	}

	/// <inheritdoc />
	public IJsonContainer? SerializeToContainer<TModel>(in TModel? model)
	{
		if (model is null) return default;
		var coreContext = new SerializerCoreContext(this);
		var token = _serializer.SerializeToNode(model, typeof(TModel), coreContext);
		if (token is null) return JsonBuilder.Object();
		if (token is IJsonContainer container) return container;

		throw new NotSupportedException();
	}

	/// <inheritdoc />
	public TContainer? SerializeToContainer<TContainer>(in object? model, in Type modelType) where TContainer : IJsonContainer
	{
		if (model is null) return default;
		var coreContext = new SerializerCoreContext(this);
		var token = _serializer.SerializeToNode(in model, in modelType, coreContext);
		if (token is null) return default;
		if (token is TContainer container) return container;

		throw new NotSupportedException();
	}
}