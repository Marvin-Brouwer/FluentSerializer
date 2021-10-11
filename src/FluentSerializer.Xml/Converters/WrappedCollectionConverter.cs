using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{
    public class WrappedCollectionConverter : IConverter<XElement>
    {
        public virtual SerializerDirection Direction => SerializerDirection.Both;
        public virtual bool CanConvert(Type targetType) =>
            !typeof(string).IsAssignableFrom(targetType) &&
            targetType.Implements(typeof(IEnumerable<>));
       
        object? IConverter<XElement>.Deserialize(XElement objectToDeserialize, ISerializerContext context)
        {
            //var collectionWrapperName = _collectionNamingStrategy.GetName(context.Property);
            //if (!objectToDeserialize.Name.ToString().Equals(collectionWrapperName, StringComparison.Ordinal))
            //    throw new IncorrectElementAccessException(context.ClassType, collectionWrapperName, objectToDeserialize.Name.ToString());

            //var instance = context.Property.PropertyType.GetEnumerableInstance()!;
            //if (objectToDeserialize.IsEmpty) return instance;

            //var itemName = context.NamingStrategy.GetName(context.Property);
            //foreach (var item in objectToDeserialize.Elements(itemName))
            //{
            //    if (item is null) continue;
            //    var itemValue = ((IAdvancedXmlSerializer)context.CurrentSerializer).Deserialize(item, context.Property.PropertyType);
            //    if (itemValue is null) continue;

            //    instance.Add(itemValue);
            //}

            //return instance;

            throw new NotImplementedException();
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