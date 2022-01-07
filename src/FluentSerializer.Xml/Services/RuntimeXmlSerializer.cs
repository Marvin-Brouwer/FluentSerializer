using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using FluentSerializer.Xml.Exceptions;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Xml.Services;

public sealed class RuntimeXmlSerializer : IAdvancedXmlSerializer
{
	private readonly XmlTypeSerializer _serializer;
	private readonly XmlTypeDeserializer _deserializer;
	private readonly ObjectPool<ITextWriter> _stringBuilderPool;

	public XmlSerializerConfiguration XmlConfiguration { get; }
	public SerializerConfiguration Configuration => XmlConfiguration;

	public RuntimeXmlSerializer(
		IScanList<(Type type, SerializerDirection direction), IClassMap> mappings, 
		XmlSerializerConfiguration configuration,
		ObjectPoolProvider objectPoolProvider)
	{
		Guard.Against.Null(mappings, nameof(mappings));
		Guard.Against.Null(configuration, nameof(configuration));

		_serializer = new XmlTypeSerializer(mappings);
		_deserializer = new XmlTypeDeserializer(mappings);
		_stringBuilderPool = objectPoolProvider.CreateLowAllocationStringBuilderPool(configuration);

		XmlConfiguration = configuration;
	}

	public TModel? Deserialize<TModel>([MaybeNull, AllowNull] IXmlElement? element)
		where TModel : new()
	{
		if (element is null) return default;

		if (typeof(IEnumerable).IsAssignableFrom(typeof(TModel))) throw new MalConfiguredRootNodeException(typeof(TModel));
		return _deserializer.DeserializeFromElement<TModel>(element, this);
	}

	public object? Deserialize([MaybeNull, AllowNull] IXmlElement? element, Type modelType)
	{
		if (element is null) return default;

		return _deserializer.DeserializeFromElement(element, modelType,  this);
	}

	public TModel? Deserialize<TModel>([MaybeNull, AllowNull] string? stringData)
		where TModel : new()
	{
		if (string.IsNullOrWhiteSpace(stringData)) return default;

		var rootElement = XmlParser.Parse(stringData);
		return Deserialize<TModel>(rootElement);
	}

	public string Serialize<TModel>([MaybeNull, AllowNull] TModel? model)
		where TModel : new()
	{
		if (model is null) return string.Empty;
		if (model is IEnumerable) throw new MalConfiguredRootNodeException(model.GetType());

		var document = SerializeToDocument(model);

		var stringValue = document.WriteTo(_stringBuilderPool, Configuration.FormatOutput, Configuration.WriteNull);

		return stringValue;
	}

	public IXmlDocument SerializeToDocument<TModel>([MaybeNull, AllowNull] TModel? model)
	{
		var rootElement = SerializeToElement(model);

		return new XmlDocument(rootElement);
	}

	public IXmlElement? SerializeToElement<TModel>([MaybeNull, AllowNull] TModel? model)
	{
		if (model is null) return default;

		return _serializer.SerializeToElement(model, typeof(TModel), this);
	}

	public IXmlElement? SerializeToElement([MaybeNull, AllowNull] object? model, Type modelType)
	{
		if (model is null) return default;

		return _serializer.SerializeToElement(model, modelType, this);
	}
}