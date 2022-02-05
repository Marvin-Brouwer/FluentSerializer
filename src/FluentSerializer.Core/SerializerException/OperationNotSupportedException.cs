using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// These exceptions are thrown when an invalid operation is executed within FluentSerializers
/// </summary>
public abstract class OperationNotSupportedException : SerializerException
{
	/// <inheritdoc />
	protected OperationNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context){ }
	/// <inheritdoc />
	protected OperationNotSupportedException(string message) : base(message) { }
}