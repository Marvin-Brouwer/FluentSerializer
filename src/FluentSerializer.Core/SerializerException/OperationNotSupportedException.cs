using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException
{
    public abstract class OperationNotSupportedException : SerializerException
    {
        protected OperationNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context){ }
        protected OperationNotSupportedException(string message) : base(message) { }
    }
}
