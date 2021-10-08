using System;
using System.Linq.Expressions;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class XmlProfileBuilder<TModel> where TModel : new()
    {
        private readonly INamingStrategy _defaultNamingStrategy;
        private readonly INamingStrategy _rootNamingStrategy;

        public XmlProfileBuilder(INamingStrategy defaultNamingStrategy, INamingStrategy rootNamingStrategy)
        {
            _defaultNamingStrategy = defaultNamingStrategy;
            _rootNamingStrategy = rootNamingStrategy;
        }

        public XmlProfileBuilder<TModel> Attribute<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            INamingStrategy? namingStrategy = null,
            ICustomAttributeConverter? converter = null
        )
        {
            throw new NotImplementedException();
        }

        public XmlProfileBuilder<TModel> Child<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            INamingStrategy? namingStrategy = null,
            ICustomElementConverter? converter = null
        )
        {
            throw new NotImplementedException();
        }
    }
}
