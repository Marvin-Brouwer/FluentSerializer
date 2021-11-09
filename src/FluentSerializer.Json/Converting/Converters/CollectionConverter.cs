﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Converting.Converters
{
    public class CollectionConverter : IJsonConverter
    {
        public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) =>
            !typeof(string).IsAssignableFrom(targetType) &&
            targetType.Implements(typeof(IEnumerable<>));

        public object? Deserialize(IJsonNode objectToDeserialize, ISerializerContext context)
        {
            if (objectToDeserialize is not IJsonArray arrayToDeserialize)
                throw new NotSupportedException($"The json object you attempted to deserialize was not a collection");

            var targetType = context.PropertyType;
            var instance = targetType.GetEnumerableInstance();

            var genericTargetType = context.PropertyType.IsGenericType
                ? context.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                : instance.GetEnumerator().Current?.GetType() ?? typeof(object);
            
            foreach (var item in arrayToDeserialize.Children)
            {
                // This will skip comments
                if (item is not IJsonContainer container) continue;

                var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).Deserialize(container, genericTargetType);
                if (itemValue is null) continue;

                instance.Add(itemValue);
            }

            return instance;
        }

        public IJsonNode Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is not IEnumerable enumerableToSerialize)
                throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' does not implement IEnumerable");

            // todo convert to select
            var customElement = new List<IJsonArrayContent>();
            foreach (var collectionItem in enumerableToSerialize)
            {
                if (collectionItem is null) continue;
                var itemValue = ((IAdvancedJsonSerializer)context.CurrentSerializer).SerializeToContainer<IJsonContainer>(collectionItem, collectionItem.GetType());
                if (itemValue is not IJsonArrayContent arrayItem) continue;

                customElement.Add(arrayItem);
            }

            return Array(customElement);
        }
    }
}