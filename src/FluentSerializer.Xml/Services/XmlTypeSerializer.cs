using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.DataNodes;
using System.Collections.Generic;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeSerializer
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;

        public XmlTypeSerializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings)
        {
            Guard.Against.Null(mappings, nameof(mappings));

            _mappings = mappings;
        }

        public IXmlElement? SerializeToElement(object dataModel, Type classType, IXmlSerializer currentSerializer)
        {
            Guard.Against.Null(dataModel, nameof(dataModel));
            Guard.Against.Null(classType, nameof(classType));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(classType);

            var classMap = _mappings.Scan((classType, SerializerDirection.Serialize));
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            var elementName = classMap.NamingStrategy.SafeGetName(classType, new NamingContext(_mappings));
            var childNodes = new List<IXmlNode>();
            foreach(var property in classType.GetProperties())
            {
                var propertyMapping = classMap.PropertyMaps.Scan(property);
                if (propertyMapping is null) continue;
                if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

                var propertyValue = property.GetValue(dataModel);
                if (propertyValue is null) continue;

                var serializerContext = new SerializerContext(
                    propertyMapping.Property, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                var childNode = SerializeProperty(propertyValue, propertyMapping, currentSerializer, serializerContext);
                if (childNode is not null) childNodes.Add(childNode);
            }

            return Element(elementName, childNodes);
        }

        private IXmlNode? SerializeProperty(
            object propertyValue, IPropertyMap propertyMapping,  
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
        {
            if (typeof(IXmlText).IsAssignableFrom(propertyMapping.ContainerType))
            {
                return SerializeNode<IXmlText>(propertyValue, propertyMapping, serializerContext);
            }

            if (typeof(IXmlAttribute).IsAssignableFrom(propertyMapping.ContainerType))
            {
                return SerializeNode<IXmlAttribute>(propertyValue, propertyMapping, serializerContext);
            }

            if (typeof(IXmlElement).IsAssignableFrom(propertyMapping.ContainerType))
            {
                return SerializeXElement(propertyValue, propertyMapping, serializerContext, currentSerializer);
            }

            throw new ContainerNotSupportedException(propertyMapping.ContainerType);
        }

        private static TNode? SerializeNode<TNode>(
            object propertyValue, IPropertyMap propertyMapping, 
            SerializerContext serializerContext)
            where TNode : class, IXmlNode
        {
            var matchingConverter = propertyMapping.GetConverter<TNode>(
                SerializerDirection.Serialize, serializerContext.CurrentSerializer);
            if (matchingConverter is null) throw new ConverterNotFoundException(
                propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
            
            return matchingConverter.Serialize(propertyValue, serializerContext);
        }

        private IXmlElement? SerializeXElement(object propertyValue, IPropertyMap propertyMapping,
            SerializerContext serializerContext, IXmlSerializer currentSerializer)
        {
            var matchingConverter = propertyMapping.GetConverter<IXmlElement>(
                SerializerDirection.Serialize, serializerContext.CurrentSerializer);

            return matchingConverter is null 
                ? SerializeToElement(propertyValue, serializerContext.PropertyType, currentSerializer) 
                : matchingConverter.Serialize(propertyValue, serializerContext);
        }
    }
}
