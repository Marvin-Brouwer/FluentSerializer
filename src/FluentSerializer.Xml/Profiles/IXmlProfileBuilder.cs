using System;
using System.Linq.Expressions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;

namespace FluentSerializer.Xml.Profiles
{
    public interface IXmlProfileBuilder<TModel>
        where TModel : new()
    {

        public XmlProfileBuilder<TModel> Attribute<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            INamingStrategy? namingStrategy = null,
            IConverter? converter = null
        );

        public XmlProfileBuilder<TModel> Child<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            INamingStrategy? namingStrategy = null,
            IConverter? converter = null
        );
        
        public void Text<TText>(
            Expression<Func<TModel, TText>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            IConverter? converter = null
        );
    }
}