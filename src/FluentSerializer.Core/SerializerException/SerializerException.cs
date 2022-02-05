using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This is the base for all exceptions thrown by the FluentSerializer library
/// </summary>
[Serializable]
public abstract class SerializerException : Exception
{
	/// <inheritdoc />
	protected SerializerException(SerializationInfo info, StreamingContext context) : base(info, context){ }
	/// <inheritdoc />
	protected SerializerException(string message) : base(message) { }
}