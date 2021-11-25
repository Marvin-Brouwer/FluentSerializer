using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting
{
    public interface IXmlConverter<TDataContainer> : IConverter<TDataContainer>, IXmlConverter
        where TDataContainer : class, IXmlNode
    {
        object? Deserialize(TDataContainer objectToDeserialize, IXmlElement? parent, ISerializerContext context) => 
            Deserialize(objectToDeserialize, context);
    }
    public interface IXmlConverter : IConverter
    {
    }
}
