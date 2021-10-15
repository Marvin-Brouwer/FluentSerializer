using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Services;

namespace FluentSerializer.Xml.Converting.Converters
{
    public class WrappedCollectionConverter : IConverter<XElement>
    {
        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) =>
            !typeof(string).IsAssignableFrom(targetType) &&
            targetType.Implements(typeof(IEnumerable<>));
       
        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            var targetType = context.PropertyType;
            var instance = targetType.GetEnumerableInstance();

            var genericTargetType = context.PropertyType.IsGenericType
                ? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                : instance.GetEnumerator().Current?.GetType() ?? typeof(object);

            var itemNamingStrategy = context.FindNamingStrategy(genericTargetType)
                ?? context.NamingStrategy;

            var itemName = itemNamingStrategy.GetName(genericTargetType, context);
            var elementsToDeserialize = objectToDeserialize!.Elements(itemName);
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
            if (objectToSerialize == null) return null;
            if (!(objectToSerialize is IEnumerable enumerableToSerialize)) 
                throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

            var customElement = new XElement(context.NamingStrategy.GetName(context.Property, context));
            foreach(var collectionItem in enumerableToSerialize)
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