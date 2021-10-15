using FluentSerializer.Core.Services;
using System.Xml.Linq;
using FluentSerializer.Xml.Configuration;

namespace FluentSerializer.Xml.Services
{
    public interface IXmlSerializer : ISerializer
    {
        XmlSerializerConfiguration XmlConfiguration { get; }
        TModel? Deserialize<TModel>(XElement element) where TModel: class, new ();
        XElement? SerializeToElement<TModel>(TModel model);
        XDocument SerializeToDocument<TModel>(TModel model);
    }
}
