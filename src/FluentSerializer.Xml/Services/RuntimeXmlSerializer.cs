using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using FluentSerializer.Xml.Exceptions;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Xml.Services;

/// <inheritdoc />
public sealed class RuntimeXmlSerializer : IAdvancedXmlSerializer
{
	private readonly XmlTypeSerializer _serializer;
	private readonly XmlTypeDeserializer _deserializer;
	private readonly ObjectPool<ITextWriter> _stringBuilderPool;

	/// <inheritdoc />
	public XmlSerializerConfiguration XmlConfiguration { get; }
	/// <inheritdoc />
	public SerializerConfiguration Configuration => XmlConfiguration;

	/// <inheritdoc cref="RuntimeXmlSerializer" />
	public RuntimeXmlSerializer(
		in XmlSerializerConfiguration configuration,
		in ObjectPoolProvider objectPoolProvider,
		in IClassMapCollection classMapCollection)
	{
		Guard.Against.Null(configuration, nameof(configuration));
		Guard.Against.Null(classMapCollection, nameof(classMapCollection));

		_serializer = new XmlTypeSerializer(in classMapCollection);
		_deserializer = new XmlTypeDeserializer(in classMapCollection);
		_stringBuilderPool = objectPoolProvider.CreateStringBuilderPool(configuration);

		XmlConfiguration = configuration;
	}

	/// <inheritdoc />
	public TModel? Deserialize<TModel>([MaybeNull, AllowNull] in IXmlElement? element)
		where TModel : new()
	{
		if (element is null) return default;
		if (typeof(IEnumerable).IsAssignableFrom(typeof(TModel))) throw new MalConfiguredRootNodeException(typeof(TModel));

		var context = new SerializerCoreContext<IXmlNode>(this)
		{
			RootNode = element
		};
		return _deserializer.DeserializeFromElement<TModel>(in element, context);
	}

	/// <inheritdoc />
	public object? Deserialize([MaybeNull, AllowNull] in IXmlElement? element, in Type modelType, in ISerializerCoreContext<IXmlNode> context)
	{
		if (element is null) return default;

		return _deserializer.DeserializeFromElement(in element, in modelType,  in context);
	}

	/// <inheritdoc />
	public TModel? Deserialize<TModel>([MaybeNull, AllowNull] in string? stringData)
		where TModel : new()
	{
		if (string.IsNullOrWhiteSpace(stringData)) return default;

		var rootElement = XmlParser.Parse(in stringData);
		return Deserialize<TModel>(in rootElement);
	}

	/// <inheritdoc />
	public string Serialize<TModel>([MaybeNull, AllowNull] in TModel? model)
		where TModel : new()
	{
		if (model is null) return string.Empty;
		if (model is IEnumerable) throw new MalConfiguredRootNodeException(model.GetType());

		var document = SerializeToDocument(in model);

		var stringValue = document.WriteTo(in _stringBuilderPool, Configuration.FormatOutput, Configuration.WriteNull);

		return stringValue;
	}

	/// <inheritdoc />
	public IXmlDocument SerializeToDocument<TModel>([MaybeNull, AllowNull] in TModel? model)
	{
		var rootElement = SerializeToElement(in model);

		return new XmlDocument(in rootElement);
	}

	/// <inheritdoc />
	public IXmlElement? SerializeToElement<TModel>([MaybeNull, AllowNull] in TModel? model)
	{
		if (model is null) return default;

		var context = new SerializerCoreContext(this);
		return _serializer.SerializeToElement(model, typeof(TModel), context);
	}

	/// <inheritdoc />
	public IXmlElement? SerializeToElement([MaybeNull, AllowNull] in object? model, in Type modelType)
	{
		if (model is null) return default;

		var context = new SerializerCoreContext(this);
		return _serializer.SerializeToElement(in model, in modelType, context);
	}
}