using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.Services;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Converting.Converters
{
    public class CollectionConverter : IJsonConverter
    {
        public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) =>
            !typeof(string).IsAssignableFrom(targetType) &&
            targetType.Implements(typeof(IEnumerable<>));

        public object? Deserialize(JToken objectToDeserialize, ISerializerContext context)
        {
            if (objectToDeserialize.Type != JTokenType.Array)
                throw new NotSupportedException($"The json object you attempted to deserialize was not a collection");

            var targetType = context.PropertyType;
            var instance = targetType.GetEnumerableInstance();

            var genericTargetType = context.PropertyType.IsGenericType
                ? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                : instance.GetEnumerator().Current?.GetType() ?? typeof(object);
            
            foreach (var item in objectToDeserialize.Children<JObject>())
            {
                var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(item, genericTargetType);
                if (itemValue is null) continue;

                instance.Add(itemValue);
            }

            return instance;
        }

        public JToken Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is not IEnumerable enumerableToSerialize)
                throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

            var customElement = new JArray();
            foreach (var collectionItem in enumerableToSerialize)
            {
                if (collectionItem is null) continue;
                var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).SerializeToContainer<JContainer>(collectionItem, collectionItem.GetType());
                if (itemValue is null) continue;

                customElement.Add(itemValue);
            }

            return customElement;
        }
    }
}