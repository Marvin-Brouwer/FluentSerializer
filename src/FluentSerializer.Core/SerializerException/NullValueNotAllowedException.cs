using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This exception will be thrown when the result of a converter returns null
/// and the property is not attributed to allow null values
/// </summary>
[Serializable]
public sealed class NullValueNotAllowedException : OperationNotSupportedException
{
	/// <summary>
	/// The concrete type of the property being assigned to
	/// </summary>
	public Type PropertyType { get; }
	/// <summary>
	/// The name of the property being assigned to
	/// </summary>
	public string TargetName { get; }

	/// <inheritdoc />
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

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(PropertyType), PropertyType);
		info.AddValue(nameof(TargetName), TargetName);

		base.GetObjectData(info, context);
	}
	#endregion
}