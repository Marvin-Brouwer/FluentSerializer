using System;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting.Converters.XNodes;
using FluentSerializer.Xml.Services;

namespace FluentSerializer.Xml.Converting.Converters
{
    public class NonWrappedCollectionConverter : IXmlConverter<XElement>
    {
        public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) => targetType.IsEnumerable();

        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            var targetType = context.PropertyType;
            var instance = targetType.GetEnumerableInstance();

            var genericTargetType = context.PropertyType.IsGenericType
                ? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                : instance.GetEnumerator().Current?.GetType() ?? typeof(object);

            var itemNamingStrategy = context.FindNamingStrategy(genericTargetType)
                ?? context.NamingStrategy;

            var itemName = itemNamingStrategy.SafeGetName(genericTargetType, context);
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
            if (objectToSerialize is not IEnumerable enumerableToSerialize)
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