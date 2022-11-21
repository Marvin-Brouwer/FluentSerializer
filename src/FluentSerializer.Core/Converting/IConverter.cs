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
/// A service implementation responsible for converting Text to and from a specific data type
/// </summary>
public interface IConverter
{
	/// <summary>
	/// Test whether this converter can convert <paramref name="targetType"/> 
	/// </summary>
	bool CanConvert(in Type targetType);

	/// <summary>
	/// The direction(s) this converter is allowed to be applied to
	/// </summary>
	SerializerDirection Direction { get; }

	/// <summary>
	/// The internal id used for the dataType attached to this converter.
	/// This is used to distinguish between converters when appending to the set of registered converters. <br/>
	/// See <see cref="ConverterComparer.Default"/> for a reference on how this is used
	/// </summary>
	/// <remarks>
	/// Even though you don't have to, it's recommended to return <c>typeof(T).GUID</c> so you don't need to manage Guids. <br/>
	/// When overriding an OOTB type like the serializer for <see cref="DateTime"/> returning <c>typeof(DateTime).GUID</c> will override the registered <see cref="IConverter"/>
	/// </remarks>
	Guid ConverterId { get; }

}