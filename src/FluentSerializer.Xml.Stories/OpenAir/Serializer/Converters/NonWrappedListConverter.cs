﻿using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters
{
    public class NonWrappedListConverter : IConverter<XElement>
    {
        public SerializerDirection Direction => SerializerDirection.Both;

        public bool CanConvert(PropertyInfo propertyInfo) => typeof(IEnumerable<>).IsAssignableFrom(propertyInfo.PropertyType);

        public object? Deserialize(XElement elementToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        public XElement? Serialize(object objectToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}