using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.Extensions;
using FluentSerializer.Xml.Mapping;
using System;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeSerializer
    {
        private readonly ILookup<Type, XmlClassMap> _mappings;

        public XmlTypeSerializer(ILookup<Type, XmlClassMap> mappings)
        {
            _mappings = mappings;
        }

        public XElement? SerializeToElement(object dataModel, Type classType, IXmlSerializer currentSerializer)
        {
            var classMap = _mappings[classType].SingleOrDefault();
            if (classMap is null) throw new ClassMapNotFoundException(classType);
            if (dataModel is null) return null;

            var newElement = new XElement(classMap.NamingStrategy.GetName(classType), null);
            foreach(var property in classType.GetProperties())
            {
                var propertyMapping = classMap.PropertyMapLookup[property].SingleOrDefault();
                if (propertyMapping is null) continue;
                if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

                var propertyName = propertyMapping.NamingStrategy.GetName(property);
                var serializerContext = new SerializerContext(property, classType, propertyMapping.NamingStrategy, currentSerializer);

                if (typeof(XText).IsAssignableFrom(propertyMapping.ContainerType))
                {
                    var textConverter = propertyMapping.GetMatchingConverter<XText>(SerializerDirection.Serialize, currentSerializer);
                    if (textConverter is null) 
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    newElement.Add(new XText(textConverter.Serialize(propertyValue, serializerContext)));
                    continue;
                }
                if (typeof(XAttribute).IsAssignableFrom(propertyMapping.ContainerType))
                {
                    var attributeConverter = propertyMapping.GetMatchingConverter<XAttribute>(SerializerDirection.Serialize, currentSerializer);
                    if (attributeConverter is null)
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Serialize);
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    newElement.SetAttributeValue(propertyName, attributeConverter.Serialize(propertyValue, serializerContext));
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
                    newElement.Add(customElement); 
                    continue;
                }

                throw new ContainerNotSupportedException(propertyMapping.ContainerType);
            }

            return newElement;
        }
    }
}
