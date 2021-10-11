using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Converters.XNodes;
using FluentSerializer.Xml.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{
    public class NonWrappedCollectionConverter : IConverter<XElement>
    {
        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) =>
            !typeof(string).IsAssignableFrom(targetType) &&
            targetType.Implements(typeof(IEnumerable<>));

        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            // Make sure we have the instancetype and not an abstract type
            var targetType = context.PropertyType;
            var instance = targetType.GetEnumerableInstance();

            var genericTargetType = context.PropertyType.IsGenericType
                ? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                : instance.GetEnumerator().Current?.GetType() ?? typeof(object);

            var itemNamingStrategy = context.FindNamingStrategy(genericTargetType)
                ?? context.NamingStrategy;

            var itemName = itemNamingStrategy.GetName(genericTargetType);
            var elementsToDeserialize = objectToDeserialize.Parent!.Elements(itemName);
            foreach (var item in elementsToDeserialize)
            {
                if (item is null) continue;
                var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).Deserialize(item, genericTargetType);
                if (itemValue is null) continue;

                instance.Add(itemValue);
            }

            return instance;
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (!(objectToSerialize is IEnumerable enumerableToSerialize))
                throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

            var customElement = new XFragment();
            foreach (var collectionItem in enumerableToSerialize)
            {
                if (collectionItem is null) continue;
                var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).SerializeToElement(collectionItem, collectionItem.GetType());
                if (itemValue is null) continue;

                customElement.Add(itemValue);
            }

            return customElement;
        }
    }
}