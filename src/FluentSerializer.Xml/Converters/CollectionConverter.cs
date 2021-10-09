using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{
    public abstract class CollectionConverter : IConverter<XElement>
    {
        private readonly INamingStrategy? _collectionNamingStrategy;

        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(PropertyInfo property) => typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

        public CollectionConverter(INamingStrategy? collectionNamingStrategy)
        {
            _collectionNamingStrategy = collectionNamingStrategy;
        }
       
        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            // todo figure out generic type + name, iterate through wrapper and use context.currentSerializer
        }


        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;

            // todo figure out correct naming, wrap in collection + iterate and use context.currentSerilalizer
        }
    }
}