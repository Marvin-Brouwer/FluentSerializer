using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeDeserializer
    {
        private readonly IScanList<Type, IClassMap> _mappings;

        public XmlTypeDeserializer(IScanList<Type, IClassMap> mappings)
        {
            Guard.Against.Null(mappings, nameof(mappings));

            _mappings = mappings;
        }

        public TModel? DeserializeFromObject<TModel>(XElement dataObject, IXmlSerializer currentSerializer)
           where TModel : class, new()
        {
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            var classType = typeof(TModel);
            var deserializedInstance = DeserializeFromObject(dataObject,classType,  currentSerializer);
            if (deserializedInstance is null) return null;

            return (TModel)deserializedInstance;
        }

        public object? DeserializeFromObject(XElement dataObject, Type classType,  IXmlSerializer currentSerializer)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(classType, nameof(classType));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotSupportedException(
               "An enumerable type made it past the custom converter check. \n" +
               $"Please make sure '{classType}' has a custom converter selected/configured.");

            var classMap = _mappings.Scan(classType);
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            if (classType == typeof(string)) return dataObject.ToString();

            var matchingTagName = classMap.NamingStrategy.GetName(classType, new NamingContext(_mappings));

            var instance = Activator.CreateInstance(classType);

            foreach (var propertyMapping in classMap.PropertyMaps)
            {

                var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
                var serializerContext = new SerializerContext(
                    realPropertyInfo, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                var propertyName = propertyMapping.NamingStrategy.GetName(realPropertyInfo, serializerContext);
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

                    var converter = propertyMapping.GetConverter<XText>(SerializerDirection.Deserialize, currentSerializer);
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

                    var converter = propertyMapping.GetConverter<XAttribute>(SerializerDirection.Deserialize, currentSerializer);
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

                    // Collections may be empty
                    if (xElement is null && propertyMapping.Property.PropertyType.IsEnumerable()) continue;
                    if (xElement is null && !propertyMapping.Property.IsNullable())
                            throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                    if (xElement is null)
                    {
                        SetPropertyValue(instance, propertyMapping, null);
                        continue;
                    }

                    var matchingConverter = propertyMapping.GetConverter<XElement>(SerializerDirection.Deserialize, currentSerializer);
                    if (matchingConverter is null)
                    {
                        if (xElement is null && !propertyMapping.Property.IsNullable())
                            throw new ContainerNotFouncException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
                        if (xElement is null)
                        {
                            SetPropertyValue(instance, propertyMapping, null);
                            continue;
                        }

                        var deserializedInstance = DeserializeFromObject(xElement,propertyMapping.Property.PropertyType,  currentSerializer);
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
