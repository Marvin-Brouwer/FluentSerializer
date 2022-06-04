using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This exception will be thrown when the serializer detects a reference loop.
/// </summary>
[Serializable]
public sealed class ReferenceLoopException : OperationNotSupportedException
{
	/// <summary>
	/// The concrete type of the instance containing the property
	/// </summary>
	public Type InstanceType { get; }
	/// <summary>
	/// The concrete type of the property being serialized
	/// </summary>
	public Type PropertyType { get; }
	/// <summary>
	/// The name of the property being serialized
	/// </summary>
	public string PropertyName { get; }

	/// <inheritdoc />
	public ReferenceLoopException(in Type instanceType, in Type propertyType, in string propertyName) : base(
		$"Reference loop detected in '{instanceType.FullName}.{propertyName}'")
	{
		InstanceType = instanceType;
		PropertyType = propertyType;
		PropertyName = propertyName;
	}

	#region Serializable
	private ReferenceLoopException(in SerializationInfo info, in StreamingContext context) : base(in info, in context)
	{
		InstanceType = (Type)info.GetValue(nameof(InstanceType), typeof(Type))!;
		PropertyType = (Type)info.GetValue(nameof(PropertyType), typeof(Type))!;
		PropertyName = (string)info.GetValue(nameof(PropertyName), typeof(string))!;
	}

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(InstanceType), InstanceType);
		info.AddValue(nameof(PropertyType), PropertyType);
		info.AddValue(nameof(PropertyName), PropertyName);

		base.GetObjectData(info, context);
	}
	#endregion
}