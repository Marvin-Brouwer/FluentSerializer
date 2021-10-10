using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.SerializerException;
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

        public object? DeserializeFromObject(Type classType, XElement dataObject, IXmlSerializer currentSerializer)
        {
            var classMap = _mappings[classType].SingleOrDefault();
            if (classMap is null) throw new ClassMapNotFoundException(classType);
            if (dataObject is null) return null;

            if (classType == typeof(string)) return dataObject.ToString();
            if (typeof(IEnumerable).IsAssignableFrom(classType) && classType.IsClass)
            {
                throw new NotImplementedException("TODO need to figure out how newtonsoft solves this");
            }

            var matchingTagName = classMap.NamingStrategy.GetName(classType);
            if (!dataObject.Name.ToString().Equals(matchingTagName, StringComparison.Ordinal))
                throw new IncorrectElementAccessException(classType, matchingTagName, dataObject.Name.ToString());

            var instance = Activator.CreateInstance(classType);

            foreach (var propertyMapping in classMap.PropertyMaps)
            {
                var serializerContext = new SerializerContext(propertyMapping.Property, classType, propertyMapping.NamingStrategy, currentSerializer);
                var propertyName = propertyMapping.NamingStrategy.GetName(propertyMapping.Property);
                if (propertyMapping.Direction == SerializerDirection.Serialize) continue;

                if (propertyMapping.ContainerType == typeof(XText))
                {
                    var xText = dataObject.Nodes().SingleOrDefault(node => node.NodeType == System.Xml.XmlNodeType.Text) as XText;
                    var textValue = xText?.Value;
                    if (string.IsNullOrEmpty(textValue) && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                    if (string.IsNullOrEmpty(textValue))
                    {
                        propertyMapping.Property.SetValue(instance, null);
                        continue;
                    }

                    var converter = propertyMapping.GetMatchingConverter<XText>(SerializerDirection.Deserialize, currentSerializer);
                    if (converter is null)
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Deserialize);

                    var propertyValue = converter.Deserialize(xText!, serializerContext);
                    if (propertyValue is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                    propertyMapping.Property.SetValue(instance, propertyValue);
                    continue;
                }
                if (propertyMapping.ContainerType == typeof(XAttribute))
                {
                    var xAttribute = dataObject.Attribute(propertyName);
                    var attributeValue = xAttribute?.Value;
                    if (string.IsNullOrEmpty(attributeValue) && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                    if (string.IsNullOrEmpty(attributeValue))
                    {
                        propertyMapping.Property.SetValue(instance, null);
                        continue;
                    }

                    var converter = propertyMapping.GetMatchingConverter<XAttribute>(SerializerDirection.Deserialize, currentSerializer);
                    if (converter is null)
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Deserialize);

                    var propertyValue = converter.Deserialize(xAttribute!, serializerContext);
                    if (propertyValue is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                    propertyMapping.Property.SetValue(instance, propertyValue);
                    continue;
                }
                if (propertyMapping.ContainerType == typeof(XElement))
                {
                    var xElement = dataObject.Element(propertyName);
                    if (xElement is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                    if (xElement is null)
                    {
                        propertyMapping.Property.SetValue(instance, null);
                        continue;
                    }

                    var matchingConverter = propertyMapping.GetMatchingConverter<XElement>(SerializerDirection.Deserialize, currentSerializer);
                    if (matchingConverter is null)
                    {
                        var deserializedInstance = DeserializeFromObject(propertyMapping.Property.PropertyType, xElement, currentSerializer);
                        if (deserializedInstance is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                            throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                        propertyMapping.Property.SetValue(instance, deserializedInstance);
                        continue;
                    }

                    var convertedInstance = matchingConverter.Deserialize(xElement, serializerContext);
                    if (convertedInstance is null && !typeof(Nullable<>).IsAssignableFrom(propertyMapping.Property.PropertyType))
                        throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                    propertyMapping.Property.SetValue(instance, convertedInstance);
                    continue;
                }

                throw new ContainerNotSupportedException(propertyMapping.ContainerType);
            }

            return instance;
        }
    }
}
