using FluentSerializer.Core.SerializerException;

using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Xml.Exceptions;

/// <summary>
/// This exception is thrown when an enumerable type is requested to be serialized
/// </summary>
[Serializable]
public sealed class MalConfiguredRootNodeException : OperationNotSupportedException
{
	/// <summary>
	/// Type that has been attempted to serialize
	/// </summary>
	public Type AttemptedType { get; }

	/// <inheritdoc />
	public MalConfiguredRootNodeException(in Type attemptedType) : base(
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

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(AttemptedType), AttemptedType);

		base.GetObjectData(info, context);
	}
	#endregion
}