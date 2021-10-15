using System;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Xml.Converting.Converters.XNodes
{
    public sealed class XObjectConverter : IXmlConverter<XElement>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;

        public bool CanConvert(Type targetType) => typeof(XObject).IsAssignableFrom(targetType);

        public object? Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            return objectToDeserialize;
        }

        public XElement? Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is null) return null;
            if (objectToSerialize is XObject xObjectToSerialize) return new XFragment(xObjectToSerialize);

            throw new NotSupportedException($"Type of '${objectToSerialize.GetType().FullName}' could not be converted");
        }
    }
}