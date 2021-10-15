﻿using System;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Converting.Converters.Base
{
    public abstract class SimpleTypeConverter<TObject> : IJsonConverter
    {
        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

        protected abstract string ConvertToString(TObject value);
        protected abstract TObject ConvertToDataType(string currentValue);

        protected virtual TObject? ConvertToNullableDataType(string? currentValue)
        {
            if (string.IsNullOrWhiteSpace(currentValue)) return default;

            return ConvertToDataType(currentValue);
        }
        
        public object? Deserialize(JToken objectToDeserialize, ISerializerContext context)
        {
            var stringValue = objectToDeserialize.ToString();
            return ConvertToNullableDataType(stringValue);
        }

        public JToken? Serialize(object objectToSerialize, ISerializerContext context)
        {
            var value = (TObject)objectToSerialize;
            var stringValue = ConvertToString(value);
            if (string.IsNullOrWhiteSpace(stringValue)) return new JObject(JValue.CreateNull());

            return JObject.Parse(stringValue);
        }
    }
}