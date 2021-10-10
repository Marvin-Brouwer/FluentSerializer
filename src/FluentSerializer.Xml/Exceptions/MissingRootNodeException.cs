using FluentSerializer.Core.SerializerException;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Xml.Exceptions
{
    public sealed class MissingRootNodeException : OperationNotSupportedException
    {
        public Type AttemptedType { get; }

        public MissingRootNodeException(Type attemptedType) : base(
            $"Type '{attemptedType}' implements IEnumerable. \n"+
            "XML documents require a root node, thus cannot (de)serialize collections as root node.")
        {
            AttemptedType = attemptedType;
        }
    }
}
