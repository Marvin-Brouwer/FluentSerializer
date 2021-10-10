using FluentSerializer.Core.Services;
using System;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public interface IXmlSerializer : ISerializer
    {
        TModel? Deserialize<TModel>(XElement rootElement) where TModel: class, new ();
        object? Deserialize(XElement item, Type propertyType);
        XElement? SerializeToElement<TModel>(TModel model);
        XElement? SerializeToElement(object collectionItem, Type propertyType);
        XDocument SerializeToDocument<TModel>(TModel model, XDeclaration? declaration = null);
    }
}
