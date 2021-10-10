namespace FluentSerializer.Core.SerializerException
{
    public abstract class OperationNotSupportedException : SerializerException
    {
        protected OperationNotSupportedException(string message) : base(message) { }
    }
}
