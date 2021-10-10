using System;

namespace FluentSerializer.Core.SerializerException
{
    public abstract class SerializerException : Exception
    {
        protected SerializerException(string message) : base(message) { }
    }
}
