using FluentSerializer.Core;
using System.Xml.Linq;

namespace FluentSerializer.Xml
{
    public interface IXmlSerializer : ISerializer
    {
        TModel Deserialize<TModel>(XObject dataObject);
        XElement SerializeToElement<TModel>(TModel dataObject);
        XDocument SerializeToDocument<TModel>(TModel dataObject);
    }
}
