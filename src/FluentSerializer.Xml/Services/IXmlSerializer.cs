using FluentSerializer.Core;
using FluentSerializer.Core.Configuration;
using System.Xml.Linq;

namespace FluentSerializer.Xml
{
    public interface IXmlSerializer : ISerializer
    {
        SerializerConfiguration Configuration { get; }

        TModel? Deserialize<TModel>(XElement rootElement) where TModel: class, new ();
        XElement? SerializeToElement<TModel>(TModel model);
        XDocument SerializeToDocument<TModel>(TModel model, XDeclaration? declaration = null);
    }
}
