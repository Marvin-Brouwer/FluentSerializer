using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeSerializer
    {
        private readonly IScanList<Type, IClassMap> _mappings;

        public XmlTypeSerializer(IScanList<Type, IClassMap> mappings)
        {
            _mappings = mappings;
        }

        public XElement? SerializeToElement(object dataModel, Type classType, IXmlSerializer currentSerializer)
        {
            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotSupportedException(
               "An enumerable type made it past the custom converter check. \n" +
               $"Please make sure '{classType}' has a custom converter selected/configured.");

            var classMap = _mappings.Scan(classType);
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            var newElement = new XElement(classMap.NamingStrategy.GetName(classType));
            foreach(var property in classType.GetProperties())
            {
                var propertyMapping = classMap.PropertyMaps.Scan(property);
                if (propertyMapping is null) continue;
                if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

                // todo intialize property here so there's always a value to serialize into
                var propertyName = propertyMapping.NamingStrategy.GetName(property);
                var serializerContext = new SerializerContext(
                    propertyMapping.Property, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                if (typeof(XText).IsAssignableFrom(propertyMapping.ContainerType))
                {
                    var textConverter = propertyMapping.GetMatchingConverter<XText>(SerializerDirection.Serialize, currentSerializer);
                    if (textConverter is null) 
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;
                    var serializedPropertyValue = textConverter.Serialize(propertyValue, serializerContext);
                    if (serializedPropertyValue is null) continue;

                    newElement.Add(new XText(serializedPropertyValue));
                    continue;
                }
                if (typeof(XAttribute).IsAssignableFrom(propertyMapping.ContainerType))
                {
                    var attributeConverter = propertyMapping.GetMatchingConverter<XAttribute>(SerializerDirection.Serialize, currentSerializer);
                    if (attributeConverter is null)
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    newElement.Add(attributeConverter.Serialize(propertyValue, serializerContext));
                    continue;
                }
                if (typeof(XElement).IsAssignableFrom(propertyMapping.ContainerType))
                {
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    var matchingConverter = propertyMapping.GetMatchingConverter<XElement>(SerializerDirection.Serialize, currentSerializer);
                    if (matchingConverter is null)
                    {
                        newElement.Add(SerializeToElement(propertyValue, property.PropertyType, currentSerializer));
                        continue;
                    }

                    var customElement = matchingConverter.Serialize(propertyValue, serializerContext);
                    if (customElement is null) continue;
                    // Special case for fragments
                    if (customElement.NodeType == XmlNodeType.DocumentFragment)
                        newElement.Add(customElement.Nodes().Where(node => node.NodeType != XmlNodeType.EndElement));
                    else 
                        newElement.Add(customElement);
                    continue;
                }

                throw new ContainerNotSupportedException(propertyMapping.ContainerType);
            }

            return newElement;
        }
    }
}
