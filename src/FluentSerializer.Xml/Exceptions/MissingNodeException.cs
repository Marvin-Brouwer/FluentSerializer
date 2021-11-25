using FluentSerializer.Core.SerializerException;
using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Xml.Exceptions
{
    [Serializable]
    public sealed class MissingNodeException : OperationNotSupportedException
    {
        public Type AttemptedType { get; }

        public MissingNodeException(Type attemptedType, string nodeName) : base(
            $"Cannot find node with name '{nodeName}' for type '{attemptedType.FullName}'")
        {
            AttemptedType = attemptedType;
        }

        #region Serializable
        private MissingNodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            AttemptedType = (Type)info.GetValue(nameof(AttemptedType), typeof(Type))!;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(AttemptedType), AttemptedType);

            base.GetObjectData(info, context);
        }
        #endregion
    }
}
