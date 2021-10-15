using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Exceptions;

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

        public XElement? SerializeToElement(object dataModel, Type classType, IXmlSerializer currentSerializer)
        {
            Guard.Against.Null(dataModel, nameof(dataModel));
            Guard.Against.Null(classType, nameof(classType));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(classType);

            var classMap = _mappings.Scan((classType, SerializerDirection.Serialize));
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            var newElement = new XElement(classMap.NamingStrategy.SafeGetName(classType, new NamingContext(_mappings)));
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

                SerializeProperty(propertyValue, newElement, propertyMapping, currentSerializer, serializerContext);
            }

            return newElement;
        }

        private void SerializeProperty(
            object propertyValue, XElement newElement, IPropertyMap propertyMapping,  
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
        {
            if (typeof(XText).IsAssignableFrom(propertyMapping.ContainerType))
            {
                SerializeXNode<XText>(propertyValue, propertyMapping, newElement, serializerContext);
                return;
            }

            if (typeof(XAttribute).IsAssignableFrom(propertyMapping.ContainerType))
            {
                SerializeXNode<XAttribute>(propertyValue, propertyMapping, newElement, serializerContext);
                return;
            }

            if (typeof(XElement).IsAssignableFrom(propertyMapping.ContainerType))
            {
                SerializeXElement(propertyValue, propertyMapping, newElement, serializerContext, currentSerializer);
                return;
            }

            throw new ContainerNotSupportedException(propertyMapping.ContainerType);
        }

        private static void SerializeXNode<TNode>(
            object propertyValue, IPropertyMap propertyMapping, 
            XElement targetElement, SerializerContext serializerContext)
            where TNode : XObject
        {
            var matchingConverter = propertyMapping.GetConverter<TNode>(
                SerializerDirection.Serialize, serializerContext.CurrentSerializer);
            if (matchingConverter is null) throw new ConverterNotFoundException(
                propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
            
            var nodeValue = matchingConverter.Serialize(propertyValue, serializerContext);
            if (nodeValue is null) return;

            targetElement.Add(nodeValue);
        }

        private void SerializeXElement(object propertyValue, IPropertyMap propertyMapping,
            XElement newElement,
            SerializerContext serializerContext, IXmlSerializer currentSerializer)
        {
            var matchingConverter = propertyMapping.GetConverter<XElement>(
                SerializerDirection.Serialize, serializerContext.CurrentSerializer);

            var nodeValue = matchingConverter is null 
                ? SerializeToElement(propertyValue, serializerContext.PropertyType, currentSerializer) 
                : matchingConverter.Serialize(propertyValue, serializerContext);
            if (nodeValue is null) return;

            // Special case for fragments
            // This is necessary because of XMLs possibility to add multiple children of the same node name
            // without it being a collection
            if (nodeValue.NodeType == XmlNodeType.DocumentFragment)
                newElement.Add(nodeValue.Nodes().Where(node => node.NodeType != XmlNodeType.EndElement));
            else 
                newElement.Add(nodeValue);
        }
    }
}
