using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public sealed class ContainerNotSupportedException : OperationNotSupportedException
{
	public Type ContainerType { get; }

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

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(ContainerType), ContainerType);

		base.GetObjectData(info, context);
	}
	#endregion
}