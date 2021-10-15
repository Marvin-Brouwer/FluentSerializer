using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Xml.Profiles
{
    public interface IXmlProfileBuilder<TModel>
        where TModel : new()
    {

        public XmlProfileBuilder<TModel> Attribute<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            Func<INamingStrategy>? namingStrategy = null,
            Func<IConverter<XAttribute>>? converter = null
        );

        public XmlProfileBuilder<TModel> Child<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            Func<INamingStrategy>? namingStrategy = null,
            Func<IConverter<XElement>>? converter = null
        );
        
        public void Text<TText>(
            Expression<Func<TModel, TText>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            Func<IConverter<XText>>? converter = null
        );
    }
}