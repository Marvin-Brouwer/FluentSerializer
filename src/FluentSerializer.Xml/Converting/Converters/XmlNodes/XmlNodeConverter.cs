using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System;

namespace FluentSerializer.Xml.Converting.Converters.XmlNodes
{
    public sealed class XmlNodeConverter : IXmlConverter<IXmlElement>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;

        public bool CanConvert(Type targetType) => typeof(IXmlNode).IsAssignableFrom(targetType);

        public object? Deserialize(IXmlElement objectToDeserialize, ISerializerContext context)
        {
            if (context.PropertyType.IsInstanceOfType(objectToDeserialize)) 
                throw new NotSupportedException($"Type of '${objectToDeserialize.GetType().FullName}' is not assignable to {context.PropertyType}");

            return objectToDeserialize;
        }

        public IXmlElement? Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is IXmlNode element) return new XmlFragment(element);

            throw new NotSupportedException($"Type of '${objectToSerialize.GetType().FullName}' could not be converted");
        }
    }
}
