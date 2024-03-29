using FluentSerializer.Core.Converting;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Services;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Context;

/// <summary>
/// Current core context for serializing data
/// </summary>
public interface ISerializerCoreContext
{
	/// <summary>
	/// The current serializer being used
	/// </summary>
	ISerializer CurrentSerializer { get; }

	/// <summary>
	/// Additional data storage to be used within custom converters.
	/// You can put anything you want in here
	/// </summary>
	/// <remarks>
	/// Exists within the current serializer run, so a new empty data collection is created
	/// every time <see cref="ISerializer.Serialize{TModel}"/> or
	/// <see cref="ISerializer.Deserialize{TModel}"/> is called.
	/// </remarks>
	IDictionary<string, TValue> Data<TSerializer, TValue>() where TSerializer : IConverter;

	/// <summary>
	/// The current class path of the serializer
	/// </summary>
	public IReadOnlyCollection<string> Path { get; }

	/// <summary>
	/// Create a copy of this context with an additional path segment
	/// </summary>
	ISerializerCoreContext WithPathSegment(in PropertyInfo propertyInfo);

	/// <inheritdoc cref="WithPathSegment(in PropertyInfo)"/>
	ISerializerCoreContext WithPathSegment(in Type type);

	/// <summary>
	/// Add an instance to the reference collection if not present already.
	/// </summary>
	bool TryAddReference(in object? instance);

	/// <summary>
	/// Check if a reference has been processed by the serializer already
	/// </summary>
	bool ContainsReference(in object? instance);
}

/// <summary>
/// Current context for serializing data
/// </summary>
public interface ISerializerCoreContext<out TDataNode> : ISerializerCoreContext where TDataNode : IDataNode
{
	/// <summary>
	/// Get the root node
	/// </summary>
	TDataNode RootNode { get; }

	/// <inheritdoc cref="ISerializerCoreContext.WithPathSegment(in PropertyInfo)"/>
	new ISerializerCoreContext<TDataNode> WithPathSegment(in PropertyInfo propertyInfo);

	/// <inheritdoc cref="ISerializerCoreContext.WithPathSegment(in PropertyInfo)"/>
	new ISerializerCoreContext<TDataNode> WithPathSegment(in Type type);
}