using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;

using System;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Core.Converting;


/// <summary>
/// A service implementation responsible for converting Text to and from <typeparamref name="TSerialContainer"/>
/// </summary>
public interface IConverter<TSerialContainer, TDataNode> : IConverter
	where TSerialContainer : IDataNode
	where TDataNode : IDataNode
{
	/// <summary>
	/// Convert <paramref name="objectToSerialize"/> to <typeparamref name="TSerialContainer"/>
	/// </summary>
	[return: MaybeNull] TSerialContainer? Serialize(in object objectToSerialize, in ISerializerContext context);

	/// <summary>
	/// Convert <paramref name="objectToDeserialize"/> to the correct instance value
	/// </summary>
	[return: MaybeNull] object? Deserialize(in TSerialContainer objectToDeserialize, in ISerializerContext<TDataNode> context);
}

/// <summary>
/// A service implementation responsible for converting Text to and from a specific datatype
/// </summary>
public interface IConverter : IComparable
{
	/// <summary>
	/// Test whether this converter can convert <paramref name="targetType"/> 
	/// </summary>
	bool CanConvert(in Type targetType);

	/// <summary>
	/// The direction(s) this converter is allowed to be applied to
	/// </summary>
	SerializerDirection Direction { get; }

	/// <inheritdoc cref="object.GetHashCode()"/>
	int GetHashCode() => ConverterHashCode;

	/// <summary>
	/// The internal hashcode used for the dataType attached to this converter.
	/// This is mostly used to distinguish between converters when appending to the set.
	/// </summary>
	int ConverterHashCode { get; }

	int IComparable.CompareTo(object? obj)
	{
		if (obj is null) return 0;
		if (obj is not IConverter converter) throw new NotSupportedException("Comparing is only supported with other IConverters");

		return converter.ConverterHashCode;
	}
}