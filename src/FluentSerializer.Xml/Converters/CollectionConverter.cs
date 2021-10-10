using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{
    public class CollectionConverter : IConverter<XElement>
    {
        private readonly INamingStrategy _collectionNamingStrategy;

        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(PropertyInfo property) =>
            !typeof(string).IsAssignableFrom(property.PropertyType) &&
            typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

        public CollectionConverter(INamingStrategy? collectionNamingStrategy = null)
        {
            _collectionNamingStrategy = new VerbatimGenericNamingStrategy();
        }
       
        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            // todo figure out generic type + name, iterate through wrapper and use context.currentSerializer

            var collectionWrapperName = _collectionNamingStrategy.GetName(context.Property);
            if (!objectToDeserialize.Name.ToString().Equals(collectionWrapperName, StringComparison.Ordinal))
                throw new NotSupportedException("Todo custom exception");

            var instance = GetEnumerableInstance(context.Property.PropertyType)!;
            if (objectToDeserialize.IsEmpty) return instance;

            var itemName = context.NamingStrategy.GetName(context.Property);
            foreach (var item in objectToDeserialize.Elements(itemName))
            {
                if (item is null) continue;
                var itemValue = ((IXmlSerializer)context.CurrentSerializer).Deserialize(item, context.Property.PropertyType);
                if (itemValue is null) continue;

                instance.Add(itemValue);
            }

            return instance;
        }

        private IList GetEnumerableInstance(Type propertyType)
        {
            if (typeof(Array).IsAssignableFrom(propertyType)) return (IList)Activator.CreateInstance(propertyType)!;
            if (typeof(ArrayList).IsAssignableFrom(propertyType)) return (IList)Activator.CreateInstance(propertyType)!;
            if (typeof(List<>).IsAssignableFrom(propertyType)) return (IList)Activator.CreateInstance(propertyType)!;

            if (propertyType.IsInterface && propertyType.IsAssignableFrom(typeof(IEnumerable<>))) {
                var listType = typeof(List<>);
                var genericType = propertyType.GetTypeInfo().GenericTypeArguments;
                return (IList)Activator.CreateInstance(listType.MakeGenericType(genericType))!;
            }

            throw new NotSupportedException("TOdo custom exception");
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize == null) return null;
            var collectionWrapperName = _collectionNamingStrategy.GetName(context.Property);
            if (!(objectToSerialize is IEnumerable enumerableToSerialize)) throw new NotSupportedException("TOdo custom exception");

            var wrapper = new XElement(collectionWrapperName);
            foreach(var collectionItem in enumerableToSerialize)
            {
                if (collectionItem is null) continue;
                var itemValue = ((IXmlSerializer)context.CurrentSerializer).SerializeToElement(collectionItem, context.Property.PropertyType);
                if (itemValue is null) continue;

                wrapper.Add(itemValue);
            }

            return wrapper;
        }
    }
}