using Ardalis.GuardClauses;

using FluentSerializer.Core.Converting;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Context;

/// <inheritdoc cref="ISerializerCoreContext"/>
public class SerializerCoreContext : ISerializerCoreContext
{
	private readonly Dictionary<string, IDictionary?> _additionalData;
	private readonly List<string> _path;
	private readonly Hashtable _referenceCollection;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Path => _path;
	/// <inheritdoc />
	public ISerializer CurrentSerializer { get; }

	/// <inheritdoc cref="ISerializerCoreContext"/>
	public SerializerCoreContext(in ISerializer currentSerializer)
	{
		_additionalData = new();
		_path = new();
		_referenceCollection = new Hashtable(currentSerializer.Configuration.ReferenceComparer);
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
		_referenceCollection = serializerCoreContext._referenceCollection;
	}

	/// <inheritdoc />
	public ISerializerCoreContext WithPathSegment(in PropertyInfo propertyInfo)
	{
		const string propertyTag = "P:";
		Guard.Against.Null(propertyInfo
#if NETSTANDARD2_1
			, nameof(propertyInfo)
#endif
		);

		return new SerializerCoreContext(this, string.Concat(propertyTag, propertyInfo.Name));
	}

	/// <inheritdoc />
	public ISerializerCoreContext WithPathSegment(in Type type)
	{
		const string typeTag = "T:";
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);

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

		return (IDictionary<string, TValue>)_additionalData[dataKey]!;
	}

	/// <inheritdoc />
	public bool TryAddReference(in object? instance)
	{
		if (instance is null) return false;

		var hash = CurrentSerializer.Configuration.ReferenceComparer.GetHashCode(instance);
		if(_referenceCollection.ContainsKey(hash)) return false;
		if(_referenceCollection.Contains(instance)) return false;

		_referenceCollection.Add(hash, instance);

		return true;
	}

	/// <inheritdoc />
	public bool ContainsReference(in object? instance) => instance is not null && _referenceCollection.Contains(instance);
}

/// <inheritdoc cref="ISerializerCoreContext{TDataNode}"/>
public sealed class SerializerCoreContext<TSerialContainer> : SerializerCoreContext, ISerializerCoreContext<TSerialContainer>
	where TSerialContainer : IDataNode
{
	/// <inheritdoc />
	public TSerialContainer RootNode { get; init; } = default!;


	/// <inheritdoc />
	public new ISerializerCoreContext<TSerialContainer> WithPathSegment(in PropertyInfo propertyInfo)
	{
		const string propertyTag = "P:";
		Guard.Against.Null(propertyInfo
#if NETSTANDARD2_1
			, nameof(propertyInfo)
#endif
		);

		return new SerializerCoreContext<TSerialContainer>(this, string.Concat(propertyTag, propertyInfo.Name));
	}

	/// <inheritdoc />
	public new ISerializerCoreContext<TSerialContainer> WithPathSegment(in Type type)
	{
		const string typeTag = "T:";
		Guard.Against.Null(type
#if NETSTANDARD2_1
			, nameof(type)
#endif
		);

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