using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Services;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public interface IXmlSerializer : ISerializer
    {
        TModel? Deserialize<TModel>(XElement rootElement) where TModel: class, new ();
        XElement? SerializeToElement<TModel>(TModel model);
        XDocument SerializeToDocument<TModel>(TModel model, XDeclaration? declaration = null);
    }
}
