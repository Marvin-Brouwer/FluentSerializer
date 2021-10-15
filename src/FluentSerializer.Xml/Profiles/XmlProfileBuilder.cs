using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using FluentSerializer.Xml.Constants;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class XmlProfileBuilder<TModel> : IXmlProfileBuilder<TModel>
        where TModel : new()
    {
        private readonly Func<INamingStrategy> _defaultNamingStrategy;
        private readonly List<IPropertyMap> _propertyMap;
        
        public XmlProfileBuilder(Func<INamingStrategy> defaultNamingStrategy, List<IPropertyMap> propertyMap)
        {
            Guard.Against.Null(defaultNamingStrategy, nameof(defaultNamingStrategy));
            Guard.Against.Null(propertyMap, nameof(propertyMap));

            _defaultNamingStrategy = defaultNamingStrategy;
            _propertyMap = propertyMap;
        }

        public IXmlProfileBuilder<TModel> Attribute<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            Func<INamingStrategy>? namingStrategy = null,
            Func<IXmlConverter<XAttribute>>? converter = null
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
        
        public IXmlProfileBuilder<TModel> Child<TAttribute>(
            Expression<Func<TModel, TAttribute>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            Func<INamingStrategy>? namingStrategy = null,
            Func<IXmlConverter<XElement>>? converter = null
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
            Func<IXmlConverter<XText>>? converter = null
        )
        {
            _propertyMap.Add(new PropertyMap(
                direction,
                typeof(XText),
                propertySelector.GetProperty(),
                // This isn't used but setting it to null requires a lot more code.
                () => XmlConstants.TextNodeNamingStrategy,
                converter
            ));
        }
    }
}
