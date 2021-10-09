using System;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
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
                var propertyMapping = classMap.PropertyMap[property].SingleOrDefault();
                if (propertyMapping is null) continue;
                if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

                var propertyName = propertyMapping.NamingStrategy.GetName(property);
                var serializationContext = new SerializerContext(property, propertyMapping.NamingStrategy, currentSerializer);

                if (typeof(XAttribute).IsAssignableFrom(propertyMapping.DestinationType))
                {
                    var attributeConverter = GetConverter<XAttribute>(propertyMapping);
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    newElement.SetAttributeValue(propertyName, attributeConverter.Serialize(null, propertyValue, serializationContext));
                    continue;
                }
                if (typeof(XElement).IsAssignableFrom(propertyMapping.DestinationType))
                {
                    var propertyValue = property.GetValue(dataModel);
                    if (propertyValue is null) continue;

                    if (propertyMapping.CustomConverter is null)
                    {
                        newElement.Add(SerializeToElement(propertyValue, currentSerializer));
                        continue;
                    }
                    
                    if (!propertyMapping.CustomConverter.CanConvert(property))
                        throw new NotSupportedException("Todo custom exception here");

                    if (propertyMapping.CustomConverter is IConverter<XObject> objectConverter)
                    {
                        var customObject = objectConverter.Serialize(null, propertyValue, serializationContext);
                        newElement.Add(customObject);
                        continue;
                    }
                    if (propertyMapping.CustomConverter is IConverter<XElement> elementConverter)
                    {
                        var customElement = elementConverter.Serialize(null, propertyValue, serializationContext);
                        newElement.Add(customElement); 
                        continue;
                    }

                    throw new NotSupportedException("Todo custom exception here");
                }

                throw new NotSupportedException("Todo custom exception here");
            }

            return newElement;
        }

        private IConverter<XObject> GetConverter<TSpecificTarget>(XmlPropertyMap propertyMapping)
            where TSpecificTarget : XObject
        {
            // todo lookup known converters
            var converter = propertyMapping.CustomConverter ?? null;
            if (converter is null) throw new NotSupportedException("Todo custom exception");

            if (!converter.CanConvert(propertyMapping.Property))
                throw new NotSupportedException("Todo custom exception");

            if (propertyMapping.CustomConverter is IConverter<XObject> objectConverter)
                return objectConverter;
            // todo test if possible
            if (propertyMapping.CustomConverter is IConverter<TSpecificTarget> specificConverter)
                return specificConverter as IConverter<XObject> ?? throw new NotSupportedException("Todo custom exception");

            throw new NotSupportedException("Todo custom exception");
        }
    }
}
