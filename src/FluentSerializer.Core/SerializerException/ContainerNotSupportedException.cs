using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This exception is thrown when a profile and a serializer don't support similar types. <br />
/// It's probably rare but not impossible with non-matching versions.
/// </summary>
[Serializable]
public sealed class ContainerNotSupportedException : OperationNotSupportedException
{
	/// <summary>
	/// The container type attemtped to deserialize
	/// </summary>
	public Type ContainerType { get; }

	/// <inheritdoc />
	public ContainerNotSupportedException(Type containerType) : base(
		$"The serial container of type '{containerType.Name}' is not supported by this ISerializer")
	{
		ContainerType = containerType;
	}

	#region Serializable
	private ContainerNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		ContainerType = (Type)info.GetValue(nameof(ContainerType), typeof(Type))!;
	}

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(ContainerType), ContainerType);

		base.GetObjectData(info, context);
	}
	#endregion
}