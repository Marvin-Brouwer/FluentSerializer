using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public abstract class SerializerException : Exception
{
	protected SerializerException(SerializationInfo info, StreamingContext context) : base(info, context){ }
	protected SerializerException(string message) : base(message) { }
}