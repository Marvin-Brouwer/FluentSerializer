using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Mapping;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using FluentSerializer.Xml.Constants;

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
            _propertyMap.Add(new XmlPropertyMap(
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
        public void Text<XText, TText>(
            Expression<Func<TModel, TText>> propertySelector,
            SerializerDirection direction = SerializerDirection.Both,
            IConverter? converter = null
        )
        {
            _propertyMap.Add(new XmlPropertyMap(
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
