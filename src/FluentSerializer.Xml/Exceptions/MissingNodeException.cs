using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.SerializerException;
using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Xml.Exceptions;

/// <summary>
/// This exception is thrown when the name resolved by the <see cref="INamingStrategy"/> does not result in a node in the data
/// </summary>
[Serializable]
public sealed class MissingNodeException : OperationNotSupportedException
{
	/// <summary>
	/// Type attempted to deserialize
	/// </summary>
	public Type AttemptedType { get; }

	/// <inheritdoc />
	public MissingNodeException(in Type attemptedType, in string nodeName) : base(
		$"Cannot find node with name '{nodeName}' for type '{attemptedType.FullName}'")
	{
		AttemptedType = attemptedType;
	}

	#region Serializable
	private MissingNodeException(SerializationInfo info, StreamingContext context) : base(info, context)
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