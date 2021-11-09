using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using FluentSerializer.Xml.Services;

namespace FluentSerializer.Xml.Converting.Converters
{
    public class NonWrappedCollectionConverter : IXmlConverter<IXmlElement>
    {
        public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) => targetType.IsEnumerable();

        object? IConverter<IXmlElement>.Deserialize(IXmlElement objectToDeserialize, ISerializerContext context)
        {
            throw new NotSupportedException();
        }

        object? IXmlConverter<IXmlElement>.Deserialize(IXmlElement objectToDeserialize, IXmlElement? parent, ISerializerContext context)
        {
            if (parent is null) throw new NotSupportedException("You cannot deserialize a non-wrapped selection at root level");

            var targetType = context.PropertyType;
            var instance = targetType.GetEnumerableInstance();

            var genericTargetType = context.PropertyType.IsGenericType
                ? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                : instance.GetEnumerator().Current?.GetType() ?? typeof(object);

            var itemNamingStrategy = context.FindNamingStrategy(genericTargetType)
                ?? context.NamingStrategy;

            var itemName = itemNamingStrategy.SafeGetName(genericTargetType, context);
            var elementsToDeserialize = parent!.GetChildElements(itemName);
            foreach (var item in elementsToDeserialize)
            {
                if (item is null) continue;
                var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).Deserialize(item, genericTargetType);
                if (itemValue is null) continue;

                instance.Add(itemValue);
            }

            return instance;
        }
        IXmlElement? IConverter<IXmlElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is not IEnumerable enumerableToSerialize)
                throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

            // todo select
            var customElement = new List<IXmlElement>();
            foreach (var collectionItem in enumerableToSerialize)
            {
                if (collectionItem is null) continue;
                var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).SerializeToElement(collectionItem, collectionItem.GetType());
                if (itemValue is null) continue;

                customElement.Add(itemValue);
            }

            return new XmlFragment(customElement);
        }
    }
}