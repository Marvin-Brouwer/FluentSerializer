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
using FluentSerializer.Xml.Exceptions;

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

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new MalConfiguredRootNodeException(classType);

            var classMap = _mappings.Scan(classType);
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            if (classType == typeof(string)) return dataObject.ToString();

            var matchingTagName = classMap.NamingStrategy.GetName(classType, new NamingContext(_mappings));
            if (dataObject.Name != matchingTagName) throw new MissingNodeException(classType, matchingTagName);

            var instance = Activator.CreateInstance(classType)!;
            foreach (var propertyMapping in classMap.PropertyMaps)
            {

                var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
                var serializerContext = new SerializerContext(
                    realPropertyInfo, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                var propertyName = propertyMapping.NamingStrategy.GetName(realPropertyInfo, serializerContext);
                if (propertyMapping.Direction == SerializerDirection.Serialize) continue;

                DeserializeProperty(dataObject, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
            }

            return instance;
        }

        private void DeserializeProperty(
            XElement dataObject, string propertyName, IPropertyMap propertyMapping, object instance, 
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
        {
            if (propertyMapping.ContainerType == typeof(XText))
            {
                // Technically there can be many text nodes, we just want all the text.
                var xText = new XText(string.Join(string.Empty, dataObject.Nodes()
                    .Where(node => node.NodeType == System.Xml.XmlNodeType.Text)
                    .Cast<XText>()
                    .Select(node => node?.Value ?? string.Empty)));

                DeserializeXNode(xText, xText?.Value, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
                return;
            }

            if (propertyMapping.ContainerType == typeof(XAttribute))
            {
                var xAttribute = dataObject.Attribute(propertyName);
                DeserializeXNode(xAttribute, xAttribute?.Value, propertyName, propertyMapping, instance, currentSerializer,serializerContext);
                return;
            }

            if (propertyMapping.ContainerType == typeof(XElement))
            {
                var xElement = dataObject.Element(propertyName);
                DeserializeXElement(xElement, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
                return;
            }

            throw new ContainerNotSupportedException(propertyMapping.ContainerType);
        }

        private static void DeserializeXNode<TNode>(
            TNode? xNode, string? nodeValue, string propertyName, IPropertyMap propertyMapping,object instance, 
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
            where TNode : XObject
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

            var convertedAttributeValue = converter.Deserialize(xNode!, serializerContext);
            if (convertedAttributeValue is null && !propertyMapping.Property.IsNullable())
                throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

            SetPropertyValue(instance, propertyMapping, convertedAttributeValue);
        }

        private void DeserializeXElement(
            XElement? xElement, string propertyName, IPropertyMap propertyMapping,object instance, 
            IXmlSerializer currentSerializer, SerializerContext serializerContext)
        {
            // Collections may be empty
            if (xElement is null && propertyMapping.Property.PropertyType.IsEnumerable()) return;

            if (xElement is null && !propertyMapping.Property.IsNullable())
                throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
            if (xElement is null)
            {
                SetPropertyValue(instance, propertyMapping, null);
                return;
            }

            var matchingConverter = propertyMapping.GetConverter<XElement>(SerializerDirection.Deserialize, currentSerializer);
            if (matchingConverter is null)
            {
                var deserializedInstance = DeserializeFromObject(xElement, propertyMapping.Property.PropertyType, currentSerializer);
                if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
                    throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                SetPropertyValue(instance, propertyMapping, deserializedInstance);
                return;
            }

            var convertedInstance = matchingConverter.Deserialize(xElement, serializerContext);
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
