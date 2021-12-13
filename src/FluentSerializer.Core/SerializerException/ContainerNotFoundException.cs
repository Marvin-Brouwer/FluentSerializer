using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public sealed class ContainerNotFoundException : OperationNotSupportedException
{
	public Type PropertyType { get; }
	public Type ContainerType { get; }
	public string TargetName { get; }

	public ContainerNotFoundException(Type propertyType, Type containerType, string targetName) : base(
		$"Container '{targetName}' of type '{containerType.Name}' was not found and '{targetName}' is not nullable")
	{
		PropertyType = propertyType;
		ContainerType = containerType;
		TargetName = targetName;
	}

	#region Serializable
	private ContainerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		PropertyType = (Type)info.GetValue(nameof(PropertyType), typeof(Type))!;
		ContainerType = (Type)info.GetValue(nameof(ContainerType), typeof(Type))!;
		TargetName = (string)info.GetValue(nameof(TargetName), typeof(string))!;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(PropertyType), PropertyType);
		info.AddValue(nameof(ContainerType), ContainerType);
		info.AddValue(nameof(TargetName), TargetName);

		base.GetObjectData(info, context);
	}
	#endregion
}