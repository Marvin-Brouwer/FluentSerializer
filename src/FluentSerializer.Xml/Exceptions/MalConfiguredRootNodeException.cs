using FluentSerializer.Core.SerializerException;
using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Xml.Exceptions;

[Serializable]
public sealed class MalConfiguredRootNodeException : OperationNotSupportedException
{
	public Type AttemptedType { get; }

	public MalConfiguredRootNodeException(Type attemptedType) : base(
		$"Type '{attemptedType}' implements IEnumerable. \n"+
		"XML documents require a root node, thus cannot (de)serialize collections as root node.")
	{
		AttemptedType = attemptedType;
	}

	#region Serializable
	private MalConfiguredRootNodeException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		AttemptedType = (Type)info.GetValue(nameof(AttemptedType), typeof(Type))!;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(AttemptedType), AttemptedType);

		base.GetObjectData(info, context);
	}
	#endregion
}