using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using FluentSerializer.Xml.Constants;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class XmlProfileBuilder<TModel> : IXmlProfileBuilder<TModel>
        where TModel : new()
    {
        private readonly INamingStrategy _defaultNamingStrategy;
        private readonly List<IPropertyMap> _propertyMap;

        public XmlProfileBuilder(INamingStrategy defaultNamingStrategy, List<IPropertyMap> propertyMap)
        {
            Guard.Against.Null(defaultNamingStrategy, nameof(defaultNamingStrategy));
            Guard.Against.Null(propertyMap, nameof(propertyMap));

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
            _propertyMap.Add(new PropertyMap(
                direction,
                typeof(XAttribute),
                propertySelector.GetProperty(),
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
            _propertyMap.Add(new PropertyMap(
                direction,
                typeof(XElement),
                propertySelector.GetProperty(),
                namingStrategy ?? _defaultNamingStrategy,
                converter
            ));

            return this;
        }

        /// <remarks>
        /// XML Elements can only have one text node so this should be set last and doesn't return a <see cref="XmlProfileBuilder{TModel}"/>
        /// </remarks>
        public void Text<TText>(
            Expression<Func<TModel, TText>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            IConverter? converter = null
        )
        {
            _propertyMap.Add(new PropertyMap(
                direction,
                typeof(XText),
                propertySelector.GetProperty(),
                // This isn't used but setting it to null requires a lot more code.
                XmlConstants.TextNodeNamingStrategy,
                converter
            ));
        }
    }
}
