using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Services
{
    public class JsonTypeDeserializer
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;

        public JsonTypeDeserializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings)
        {
            Guard.Against.Null(mappings, nameof(mappings));

            _mappings = mappings;
        }

        public TModel? DeserializeFromToken<TModel>(JToken dataObject, IJsonSerializer currentSerializer)
           where TModel : class, new()
        {
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            var classType = typeof(TModel);
            var deserializedInstance = DeserializeFromToken(dataObject,classType,  currentSerializer);
            if (deserializedInstance is null) return null;

            return (TModel)deserializedInstance;
        }

        public object? DeserializeFromToken(JToken dataObject, Type classType,  IJsonSerializer currentSerializer)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(classType, nameof(classType));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotImplementedException("Todo");
            if (dataObject is not JObject jObject)  throw new NotImplementedException("Todo");

            var classMap = _mappings.Scan((classType, SerializerDirection.Deserialize));
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            if (classType == typeof(string)) return dataObject.ToString();
            
            var instance = Activator.CreateInstance(classType)!;
            foreach (var propertyMapping in classMap.PropertyMaps)
            {

                var realPropertyInfo = classType.GetProperty(propertyMapping.Property.Name)!;
                var serializerContext = new SerializerContext(
                    realPropertyInfo, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                var propertyName = propertyMapping.NamingStrategy.SafeGetName(realPropertyInfo, serializerContext);
                if (propertyMapping.Direction == SerializerDirection.Serialize) continue;

                DeserializeProperty(jObject, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
            }

            return instance;
        }

        private void DeserializeProperty(
            JObject dataObject, string propertyName, IPropertyMap propertyMapping, object instance, 
            IJsonSerializer currentSerializer, SerializerContext serializerContext)
        {
            if (propertyMapping.ContainerType == typeof(JProperty))
            {
                var jProperty = dataObject.Property(propertyName);
                var propertyValue = jProperty?.Value;
                DeserializeXElement(propertyValue, propertyName, propertyMapping, instance, currentSerializer, serializerContext);
                return;
            }

            throw new ContainerNotSupportedException(propertyMapping.ContainerType);
        }

        private void DeserializeXElement(
            JToken? propertyValue, string propertyName, IPropertyMap propertyMapping,object instance, 
            IJsonSerializer currentSerializer, SerializerContext serializerContext)
        {
            var empty = propertyValue is null || propertyValue.Type == JTokenType.None 
                            || propertyValue.Type == JTokenType.Null || propertyValue.Type == JTokenType.Undefined;
            // Collections may be empty
            if (empty && propertyMapping.Property.PropertyType.IsEnumerable()) return;

            if (empty && !propertyMapping.Property.IsNullable())
                throw new ContainerNotFoundException(propertyMapping.Property.PropertyType, propertyMapping.ContainerType, propertyName);
            if (empty)
            {
                SetPropertyValue(instance, propertyMapping, null);
                return;
            }

            var matchingConverter = propertyMapping.GetConverter<JToken>(SerializerDirection.Deserialize, currentSerializer);
            if (matchingConverter is null)
            {
                var deserializedInstance = DeserializeFromToken(propertyValue!, propertyMapping.Property.PropertyType, currentSerializer);
                if (deserializedInstance is null && !propertyMapping.Property.IsNullable())
                    throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

                SetPropertyValue(instance, propertyMapping, deserializedInstance);
                return;
            }

            var convertedInstance = matchingConverter.Deserialize(propertyValue!, serializerContext);
            if (convertedInstance is null && !propertyMapping.Property.IsNullable())
                throw new NullValueNotAllowedException(propertyMapping.Property.PropertyType, propertyName);

            SetPropertyValue(instance, propertyMapping, convertedInstance);
        }

        private static void SetPropertyValue(object? instance, IPropertyMap propertyMapping, object? convertedInstance)
        {
            var propertyInstance = instance!.GetType().GetProperty(propertyMapping.Property.Name);
            propertyInstance!.SetValue(instance, convertedInstance, null);
        }
    }
}
