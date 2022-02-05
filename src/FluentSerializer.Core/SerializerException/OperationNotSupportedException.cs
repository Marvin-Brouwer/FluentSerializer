using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// These exceptions are thrown when an invalid operation is executed within FluentSerializers
/// </summary>
public abstract class OperationNotSupportedException : SerializerException
{
	/// <inheritdoc />
	protected OperationNotSupportedException(in SerializationInfo info, in StreamingContext context) : base(in info, in context) { }
	/// <inheritdoc />
	protected OperationNotSupportedException(in string message) : base(in message) { }
}