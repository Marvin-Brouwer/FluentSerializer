using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This exception will be thrown when a mapped property does not support null values AND is not present in the data. 
/// </summary>
[Serializable]
public sealed class ContainerNotFoundException : OperationNotSupportedException
{
	/// <summary>
	/// Type of the property refering to this container
	/// </summary>
	public Type PropertyType { get; }
	/// <summary>
	/// Class type that should be deserialized into
	/// </summary>
	public Type ContainerType { get; }
	/// <summary>
	/// Name of the property refering to this container
	/// </summary>
	public string TargetName { get; }

	/// <inheritdoc />
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

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(PropertyType), PropertyType);
		info.AddValue(nameof(ContainerType), ContainerType);
		info.AddValue(nameof(TargetName), TargetName);

		base.GetObjectData(info, context);
	}
	#endregion
}