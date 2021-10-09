using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
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

        public XElement? SerializeToElement<TModel>(TModel dataModel, IXmlSerializer currentSerializer)
        {
            var classType = typeof(TModel);
            var classMap = _mappings[classType].SingleOrDefault();
            if (classMap is null) throw new NotSupportedException("TODO create custom exception here");
            if (dataModel is null) return null;

            var newElement = new XElement(classMap.NamingStrategy.GetName(classType), null);
            foreach(var property in classType.GetProperties())
            {
                // Todo support multiple mappers or just remove the currentValue?
                var propertyMapping = classMap.PropertyMapLookup[property].SingleOrDefault();
                if (propertyMapping is null) continue;
                if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

                var propertyName = propertyMapping.NamingStrategy.GetName(property);
                var serializerContext = new SerializerContext(property, propertyMapping.NamingStrategy, currentSerializer);

                if (typeof(XAttribute).IsAssignableFrom(propertyMapping.DestinationType))
                {
                    var attributeConverter = propertyMapping.GetMatchingConverter<XAttribute>(currentSerializer);
                    if (attributeConverter is null) throw new NotSupportedException("Todo custom exception");
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    newElement.SetAttributeValue(propertyName, attributeConverter.Serialize(null, propertyValue, serializerContext));
                    continue;
                }
                if (typeof(XElement).IsAssignableFrom(propertyMapping.DestinationType))
                {
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    var matchingConverter = propertyMapping.GetMatchingConverter<XElement>(currentSerializer);
                    if (matchingConverter is null)
                    {
                        newElement.Add(SerializeToElement(propertyValue, currentSerializer));
                        continue;
                    }
                    
                    if (!matchingConverter.CanConvert(property))
                        throw new NotSupportedException("Todo custom exception here");

                    if (matchingConverter is IConverter<XObject> objectConverter)
                    {
                        var customObject = objectConverter.Serialize(null, propertyValue, serializerContext);
                        newElement.Add(customObject);
                        continue;
                    }
                    if (matchingConverter is IConverter<XElement> elementConverter)
                    {
                        var customElement = elementConverter.Serialize(null, propertyValue, serializerContext);
                        newElement.Add(customElement); 
                        continue;
                    }

                    throw new NotSupportedException("Todo custom exception here");
                }

                throw new NotSupportedException("Todo custom exception here");
            }

            return newElement;
        }
    }
}
