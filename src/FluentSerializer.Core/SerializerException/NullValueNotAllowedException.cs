using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public sealed class NullValueNotAllowedException : OperationNotSupportedException
{
	public Type PropertyType { get; }
	public string TargetName { get; }

	public NullValueNotAllowedException(Type propertyType, string targetName) : base(
		$"Value of '{targetName}' evaluated to null, which is not allowed for '{propertyType.FullName}'")
	{
		PropertyType = propertyType;
		TargetName = targetName;
	}

	#region Serializable
	private NullValueNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		PropertyType = (Type)info.GetValue(nameof(PropertyType), typeof(Type))!;
		TargetName = (string)info.GetValue(nameof(TargetName), typeof(string))!;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(PropertyType), PropertyType);
		info.AddValue(nameof(TargetName), TargetName);

		base.GetObjectData(info, context);
	}
	#endregion
}