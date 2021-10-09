using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Extensions;
using FluentSerializer.Xml.Mapping;
using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeDeserializer
    {
        private readonly ILookup<Type, XmlClassMap> _mappings;

        public XmlTypeDeserializer(ILookup<Type, XmlClassMap> mappings)
        {
            _mappings = mappings;
        }
        public TModel? DeserializeFromObject<TModel>(XElement dataObject, IXmlSerializer currentSerializer)
           where TModel : class, new()
        {
            var classType = typeof(TModel);
            var deserializedInstance = DeserializeFromObject(classType, dataObject, currentSerializer);
            if (deserializedInstance is null) return null;

            return (TModel)deserializedInstance;
        }
        private object? DeserializeFromObject(Type classType, XElement dataObject, IXmlSerializer currentSerializer)
        {
            var classMap = _mappings[classType].SingleOrDefault();
            if (classMap is null) throw new NotSupportedException("TODO create custom exception here");
            if (dataObject is null) return null;

            if (classType == typeof(string)) return dataObject.ToString();
            if (typeof(IEnumerable).IsAssignableFrom(classType) && classType.IsClass)
            {
                throw new NotImplementedException("TODO need to figure out how newtonsoft solves this");
            }

            var matchingTagName = classMap.NamingStrategy.GetName(classType);
            if (!dataObject.Name.ToString().Equals(matchingTagName, StringComparison.Ordinal))
                throw new NotSupportedException("TODO create custom exception here");

            var instance = Activator.CreateInstance(classType);

            foreach (var propertyMapping in classMap.PropertyMaps)
            {
                var serializerContext = new SerializerContext(propertyMapping.Property, classType, propertyMapping.NamingStrategy, currentSerializer);
                var propertyName = propertyMapping.NamingStrategy.GetName(propertyMapping.Property);

                if (propertyMapping.ContainerType == typeof(XAttribute))
                {
                    var xAttribute = dataObject.Attribute(propertyName);
                    var attributeValue = xAttribute?.Value;
                    if (string.IsNullOrEmpty(attributeValue) && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new NotSupportedException("Todo custom exception");
                    if (string.IsNullOrEmpty(attributeValue))
                    {
                        propertyMapping.Property.SetValue(instance, null);
                        continue;
                    }

                    // Todo find default converters
                    var converter = propertyMapping.GetMatchingConverter<XAttribute>(currentSerializer);
                    if (converter is null) throw new NotSupportedException("TODO create custom exception here");
                    if (!converter.CanConvert(propertyMapping.Property)) throw new NotSupportedException("TODO create custom exception here");

                    var propertyValue = converter.Deserialize(xAttribute!, serializerContext);
                    if (propertyValue is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new NotSupportedException("Todo custom exception");

                    propertyMapping.Property.SetValue(instance, propertyValue);
                    continue;
                }
                if (propertyMapping.ContainerType == typeof(XElement))
                {
                    var xElement = dataObject.Element(propertyName);
                    if (xElement is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new NotSupportedException("Todo custom exception");
                    if (xElement is null)
                    {
                        propertyMapping.Property.SetValue(instance, null);
                        continue;
                    }

                    var matchingConverter = propertyMapping.GetMatchingConverter<XElement>(currentSerializer);
                    if (matchingConverter is null)
                    {
                        var deserializedInstance = DeserializeFromObject(propertyMapping.Property.PropertyType, xElement, currentSerializer);
                        if (deserializedInstance is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                            throw new NotSupportedException("Todo custom exception");

                        propertyMapping.Property.SetValue(instance, deserializedInstance);
                        continue;
                    }

                    if (!matchingConverter.CanConvert(propertyMapping.Property))
                        throw new NotSupportedException("Todo custom exception here");

                    if (matchingConverter is IConverter<XObject> objectConverter)
                    {
                        var deserializedInstance = objectConverter.Deserialize(xElement, serializerContext);
                        if (deserializedInstance is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                            throw new NotSupportedException("Todo custom exception");

                        propertyMapping.Property.SetValue(instance, deserializedInstance);
                        continue;
                    }
                    if (matchingConverter is IConverter<XElement> elementConverter)
                    {
                        var deserializedInstance = elementConverter.Deserialize(xElement, serializerContext);
                        if (deserializedInstance is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                            throw new NotSupportedException("Todo custom exception");

                        propertyMapping.Property.SetValue(instance, deserializedInstance);
                        continue;
                    }
                    throw new NotSupportedException("Todo custom exception");
                }

                throw new NotSupportedException("Todo custom exception");
            }

            return instance;
        }
    }
}
