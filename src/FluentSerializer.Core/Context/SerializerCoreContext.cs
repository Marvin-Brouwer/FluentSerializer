using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Services;
using System;
using System.Collections;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;
using System.Runtime.CompilerServices;
using FluentSerializer.Core.DataNodes;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Converting;
using JetBrains.Annotations;

namespace FluentSerializer.Core.Context;

/// <inheritdoc cref="ISerializerCoreContext"/>
public class SerializerCoreContext : ISerializerCoreContext
{
	private readonly Dictionary<string, IDictionary?> _additionalData;
	private readonly List<string> _path;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Path => _path;
	/// <inheritdoc />
	public ISerializer CurrentSerializer { get; }

	/// <inheritdoc cref="ISerializerCoreContext"/>
	public SerializerCoreContext(in ISerializer currentSerializer)
	{
		_additionalData = new Dictionary<string, IDictionary?>();
		_path = new List<string>();
		CurrentSerializer = currentSerializer;
	}
	
	/// <inheritdoc cref="ISerializerCoreContext"/>
	protected SerializerCoreContext(SerializerCoreContext serializerCoreContext, in string name)
	{
		_additionalData = serializerCoreContext._additionalData;
		CurrentSerializer = serializerCoreContext.CurrentSerializer;
		_path = new List<string>(serializerCoreContext.Path)
		{
			name
		};
	}

	/// <inheritdoc />
	public ISerializerCoreContext WithPathSegment(in PropertyInfo property)
	{
		const string propertyTag = "P:";
		Guard.Against.Null(property, nameof(property));

		return new SerializerCoreContext(this, string.Concat(propertyTag, property.Name));
	}

	/// <inheritdoc />
	public ISerializerCoreContext WithPathSegment(in Type type)
	{
		const string typeTag = "T:";
		Guard.Against.Null(type, nameof(type));

		return new SerializerCoreContext(this, string.Concat(typeTag, type.Name));
	}

	/// <inheritdoc />
	public IDictionary<string, TValue> Data<TSerializer, TValue>() where TSerializer : IConverter
	{
		var typeName = typeof(TSerializer).FullName!;
		var valueName = typeof(TValue).FullName!;
		var dataKey = string.Concat(typeName, valueName);

		if (!_additionalData.ContainsKey(dataKey))
			_additionalData.Add(dataKey, new Dictionary<string, TValue>());

		return (IDictionary<string, TValue>) _additionalData[dataKey]!;
	}
}

/// <inheritdoc cref="ISerializerCoreContext{TDataNode}"/>
public sealed class SerializerCoreContext<TSerialContainer> : SerializerCoreContext, ISerializerCoreContext<TSerialContainer>
	where TSerialContainer : IDataNode
{
	/// <inheritdoc />
	public TSerialContainer RootNode { get; init; } = default!;


	/// <inheritdoc />
	public new ISerializerCoreContext<TSerialContainer> WithPathSegment(in PropertyInfo property)
	{
		const string propertyTag = "P:";
		Guard.Against.Null(property, nameof(property));

		return new SerializerCoreContext<TSerialContainer>(this, string.Concat(propertyTag, property.Name));
	}

	/// <inheritdoc />
	public new ISerializerCoreContext<TSerialContainer> WithPathSegment(in Type type)
	{
		const string typeTag = "T:";
		Guard.Against.Null(type, nameof(type));

		return new SerializerCoreContext<TSerialContainer>(this, string.Concat(typeTag, type.Name));
	}

	/// <inheritdoc cref="ISerializerCoreContext{TDataNode}"/>
	public SerializerCoreContext(in ISerializer currentSerializer)
		: base(in currentSerializer) { }

	/// <inheritdoc cref="ISerializerCoreContext{TDataNode}"/>
	private SerializerCoreContext(in SerializerCoreContext<TSerialContainer> coreContext, in string name)
		: base(coreContext, in name)
	{
		RootNode = coreContext.RootNode;
	}
}