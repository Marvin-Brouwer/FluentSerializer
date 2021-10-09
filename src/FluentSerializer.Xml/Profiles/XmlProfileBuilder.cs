using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class XmlProfileBuilder<TModel> : IXmlProfileBuilder
        where TModel : new()
    {
        private readonly INamingStrategy _defaultNamingStrategy;

        private readonly List<XmlPropertyMap> _propertyMap;

        public XmlProfileBuilder(INamingStrategy defaultNamingStrategy, List<XmlPropertyMap> propertyMap)
        {
            _defaultNamingStrategy = defaultNamingStrategy;
            _propertyMap = propertyMap;
        }

        public XmlProfileBuilder<TModel> Attribute<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            INamingStrategy? namingStrategy = null,
            IConverter? converter = null
        )
        {
            _propertyMap.Add(new XmlPropertyMap(
                direction,
                GetProperty(propertySelector),
                namingStrategy ?? _defaultNamingStrategy,
                converter
            ));

            return this;
        }

        public XmlProfileBuilder<TModel> Child<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            INamingStrategy? namingStrategy = null,
            IConverter? converter = null
        )
        {
            _propertyMap.Add(new XmlPropertyMap(
                direction,
                GetProperty(propertySelector),
                namingStrategy ?? _defaultNamingStrategy,
                converter
            ));

            return this;
        }

        // todo move to extension method
        public static PropertyInfo GetProperty(LambdaExpression lambda)
        {
            if (lambda.Body is UnaryExpression unaryExpression)
                return (PropertyInfo)((MemberExpression)unaryExpression.Operand).Member;
            
            return (PropertyInfo)((MemberExpression)lambda.Body).Member;
        }
    }
}
