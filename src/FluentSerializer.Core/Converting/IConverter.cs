using System;
using System.Diagnostics.CodeAnalysis;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Core.Converting;


/// <summary>
/// A service implementation responsible for converting Text to and from <typeparamref name="TSerialContainer"/>
/// </summary>
public interface IConverter<TSerialContainer> : IConverter where TSerialContainer : IDataNode
{
	/// <summary>
	/// Convert <paramref name="objectToSerialize"/> to <typeparamref name="TSerialContainer"/>
	/// </summary>
	[return: MaybeNull] TSerialContainer? Serialize(object objectToSerialize, ISerializerContext context);

	/// <summary>
	/// Convert <paramref name="objectToDeserialize"/> to the correct instance value
	/// </summary>
	[return: MaybeNull] object? Deserialize(TSerialContainer objectToDeserialize, ISerializerContext context);
}

/// <summary>
/// A service implementation responsible for converting Text to and from a specific datatype
/// </summary>
public interface IConverter
{
	/// <summary>
	/// Test whether this converter can convert <paramref name="targetType"/> 
	/// </summary>
	bool CanConvert(Type targetType);

	/// <summary>
	/// The direction(s) this converter is allowed to be applied to
	/// </summary>
	SerializerDirection Direction { get; }
}