using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Exceptions;
using System;
using System.Collections;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Services
{
    public class XmlTypeDeserializer
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;

        public XmlTypeDeserializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings)
        {
            Guard.Against.Null(mappings, nameof(mappings));

            _mappings = mappings;
        }

        public TModel? DeserializeFromElement<TModel>(IXmlElement dataObject, IXmlSerializer currentSerializer)
           where TModel : new()
        {
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            var classType = typeof(TModel);
            var deserializedInstance = DeserializeFromElement(dataObject, classType,  currentSerializer);
            if (deserializedInstance is null) return default;

            return (TModel)deserializedInstance;
        }

        public object? DeserializeFromElement(IXmlElement dataObject, Type classType,  IXmlSerializer currentSerializer)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(classType, nameof(classType));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(classType);

            var classMap = _mappings.Scan((classType, SerializerDirection.Deserialize));
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            if (classType == typeof(string)) return dataObject.ToString();

            var matchingTagName = classMap.NamingStrategy.SafeGetName(classType, new NamingContext(_mappings));
            if (dataObject.Name != matchingTagName) throw new MissingNodeException(classType, matchingTagName);

            var instance = Activator.CreateInstance(classType)!;
            foreach (var propertyMapping in classMap.PropertyMaps)
            {

                var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
                var serializerContext = new SerializerContext(
                    realPropertyInfo, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                var propertyName = propertyMapping.NamingStrategy.SafeGetName(realPropertyInfo, serializerContext);
                if (propertyMapping.Direction == SerializerDirection.Serialize) continue;

                DeserializeProperty(dataObject, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
            }

            return instance;
        }

        private void DeserializeProperty(
            IXmlElement dataObject, string propertyName, IPropertyMap propertyMapping, object instance, 
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
        {
            if (propertyMapping.ContainerType == typeof(IXmlText))
            {
                // This may look strange here but it makes DeserializeNode a lot simpler
                var text = Text(dataObject.GetTextValue());
                DeserializeNode(text, dataObject, text.Value, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
                return;
            }

            if (propertyMapping.ContainerType == typeof(IXmlAttribute))
            {
                var attribute = dataObject.GetChildAttribute(propertyName);
                DeserializeNode(attribute, dataObject, attribute?.Value, propertyName, propertyMapping, instance, currentSerializer,serializerContext);
                return;
            }

            if (propertyMapping.ContainerType == typeof(IXmlElement))
            {
                var element = dataObject.GetChildElement(propertyName);
                DeserializeElement(element, dataObject, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
                return;
            }

            throw new ContainerNotSupportedException(propertyMapping.ContainerType);
        }

        private static void DeserializeNode<TNode>(
            TNode? node, IXmlElement parent, string? nodeValue, string propertyName, IPropertyMap propertyMapping,object instance, 
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
            where TNode : IXmlNode
        {
            if (nodeValue is null && !propertyMapping.Property.IsNullable())
                throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
            if (nodeValue is null)
            {
                SetPropertyValue(instance, propertyMapping, null);
                return;
            }

            var converter = propertyMapping.GetConverter<TNode>(SerializerDirection.Deserialize, currentSerializer);
            if (converter is null) throw new ConverterNotFoundException(
                propertyMapping.Property.PropertyType, propertyMapping.ContainerType, SerializerDirection.Deserialize);

            var convertedAttributeValue = converter is IXmlConverter<TNode> xmlConverter
                ? xmlConverter.Deserialize(node!, parent, serializerContext)
                : converter.Deserialize(node!, serializerContext);
            if (convertedAttributeValue is null && !propertyMapping.Property.IsNullable())
                throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

            SetPropertyValue(instance, propertyMapping, convertedAttributeValue);
        }

        private void DeserializeElement(
            IXmlElement? element, IXmlElement parent, string propertyName, IPropertyMap propertyMapping,object instance, 
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
        {
            // Collections may be empty
            if (element is null && propertyMapping.Property.PropertyType.IsEnumerable()) return;

            if (element is null && !propertyMapping.Property.IsNullable())
                throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
            if (element is null)
            {
                SetPropertyValue(instance, propertyMapping, null);
                return;
            }

            var matchingConverter = propertyMapping.GetConverter<IXmlElement>(SerializerDirection.Deserialize, currentSerializer);
            if (matchingConverter is null)
            {
                var deserializedInstance = DeserializeFromElement(element, propertyMapping.Property.PropertyType, currentSerializer);
                if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
                    throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                SetPropertyValue(instance, propertyMapping, deserializedInstance);
                return;
            }

            var convertedInstance = matchingConverter is IXmlConverter<IXmlElement> xmlConverter
                ? xmlConverter.Deserialize(element, parent, serializerContext)
                : matchingConverter.Deserialize(element, serializerContext);
            if (convertedInstance is null && !propertyMapping.Property.IsNullable())
                throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

            SetPropertyValue(instance, propertyMapping, convertedInstance);
        }

        private static void SetPropertyValue(object? instance, IPropertyMap propertyMapping, object? convertedInstance)
        {
            var propertyInstance = instance!.GetType().GetProperty(propertyMapping.Property.Name);
            propertyInstance!.SetValue(instance, convertedInstance, null);
        }
    }
}
