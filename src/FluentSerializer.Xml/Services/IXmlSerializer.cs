using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Services
{
    public interface IXmlSerializer : ISerializer
    {
        XmlSerializerConfiguration XmlConfiguration { get; }
        TModel? Deserialize<TModel>(IXmlElement element) where TModel: class, new ();
        IXmlElement? SerializeToElement<TModel>(TModel model);
        IXmlDocument? SerializeToDocument<TModel>(TModel model);
    }
}
