using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.Converters.XNodes;
using FluentSerializer.Xml.Exceptions;
using FluentSerializer.Xml.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeDeserializer
    {
        private readonly ISearchDictionary<Type, IClassMap> _mappings;

        public XmlTypeDeserializer(ISearchDictionary<Type, IClassMap> mappings)
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
            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotSupportedException(
               "An enumerable type made it past the custom converter check. \n" +
               $"Please make sure '{classType}' has a custom converter selected/configured.");

            var classMap = _mappings.Find(classType);
            if (classMap is null) throw new ClassMapNotFoundException(classType);
            if (dataObject is null) return null;

            if (classType == typeof(string)) return dataObject.ToString();

            var matchingTagName = classMap.NamingStrategy.GetName(classType);
            if (!dataObject.Name.ToString().Equals(matchingTagName, StringComparison.Ordinal))
                throw new IncorrectElementAccessException(classType, matchingTagName, dataObject.Name.ToString());

            var instance = Activator.CreateInstance(classType);

            foreach (var propertyMapping in classMap.PropertyMaps)
            {
                var serializerContext = new SerializerContext(
                    propertyMapping.Property, classType, propertyMapping.NamingStrategy, _mappings, currentSerializer);
                var propertyName = propertyMapping.NamingStrategy.GetName(propertyMapping.Property);
                if (propertyMapping.Direction == SerializerDirection.Serialize) continue;

                if (propertyMapping.ContainerType == typeof(XText))
                {
                    var xText = dataObject.Nodes().SingleOrDefault(node => node.NodeType == System.Xml.XmlNodeType.Text) as XText;
                    var textValue = xText?.Value;
                    if (string.IsNullOrEmpty(textValue) && !propertyMapping.Property.IsNullable())
                        throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                    if (string.IsNullOrEmpty(textValue))
                    {
                        SetPropertyValue(instance, propertyMapping, null);
                        continue;
                    }

                    var converter = propertyMapping.GetMatchingConverter<XText>(SerializerDirection.Deserialize, currentSerializer);
                    if (converter is null)
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Deserialize);

                    var convertedTextValue = converter.Deserialize(xText!, serializerContext);
                    if (convertedTextValue is null && !propertyMapping.Property.IsNullable())
                        throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                    SetPropertyValue(instance, propertyMapping, convertedTextValue);
                    continue;
                }
                if (propertyMapping.ContainerType == typeof(XAttribute))
                {
                    var xAttribute = dataObject.Attribute(propertyName);
                    var attributeValue = xAttribute?.Value;
                    if (attributeValue is null && !propertyMapping.Property.IsNullable())
                        throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                    if (attributeValue is null)
                    {
                        SetPropertyValue(instance, propertyMapping, null);
                        continue;
                    }

                    var converter = propertyMapping.GetMatchingConverter<XAttribute>(SerializerDirection.Deserialize, currentSerializer);
                    if (converter is null)
                        throw new ConverterNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Deserialize);

                    var convertedAttributeValue = converter.Deserialize(xAttribute!, serializerContext);
                    if (convertedAttributeValue is null && !propertyMapping.Property.IsNullable())
                        throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                    SetPropertyValue(instance, propertyMapping, convertedAttributeValue);
                    continue;
                }
                if (propertyMapping.ContainerType == typeof(XElement))
                {
                    var xElement = dataObject.Element(propertyName);

                    // Add an empty shell to help if data is missing
                    if (xElement is null)
                    {
                        xElement = new XFragment();
                        dataObject.AddFirst(xElement);
                        var test = xElement.Parent;
                    }

                    var matchingConverter = propertyMapping.GetMatchingConverter<XElement>(SerializerDirection.Deserialize, currentSerializer);
                    if (matchingConverter is null)
                    {
                        if (xElement is null && !propertyMapping.Property.IsNullable())
                            throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                        if (xElement is null)
                        {
                            SetPropertyValue(instance, propertyMapping, null);
                            continue;
                        }

                        var deserializedInstance = DeserializeFromObject(propertyMapping.Property.PropertyType, xElement, currentSerializer);
                        if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
                            throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                        SetPropertyValue(instance, propertyMapping, deserializedInstance);
                        continue;
                    }

                    var convertedInstance = matchingConverter.Deserialize(xElement, serializerContext);
                    if (convertedInstance is null && !propertyMapping.Property.IsNullable())
                        throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                    SetPropertyValue(instance, propertyMapping, convertedInstance);
                    continue;
                }

                throw new ContainerNotSupportedException(propertyMapping.ContainerType);
            }

            return instance;
        }

        private static void SetPropertyValue(object? instance, IPropertyMap propertyMapping, object? convertedInstance)
        {
            var propertyInstance = instance!.GetType().GetProperty(propertyMapping.Property.Name);
            propertyInstance!.SetValue(instance, convertedInstance, null);
        }
    }
}
