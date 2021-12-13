using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public sealed class IncorrectElementAccessException : OperationNotSupportedException
{
	public Type ClassType { get; }
	public string TargetName { get; }
	public string ElementName { get; }

	public IncorrectElementAccessException(Type classType, string targetName, string elementName) : base(
		$"The name used for '{classType.FullName}' ({targetName}) does not match current element name '{elementName}'")
	{
		ClassType = classType;
		TargetName = targetName;
		ElementName = elementName;
	}

	#region Serializable
	private IncorrectElementAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		ClassType = (Type)info.GetValue(nameof(ClassType), typeof(Type))!;
		TargetName = (string)info.GetValue(nameof(TargetName), typeof(string))!;
		ElementName = (string)info.GetValue(nameof(ElementName), typeof(string))!;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(ClassType), ClassType);
		info.AddValue(nameof(TargetName), TargetName);
		info.AddValue(nameof(ElementName), ElementName);

		base.GetObjectData(info, context);
	}
	#endregion
}