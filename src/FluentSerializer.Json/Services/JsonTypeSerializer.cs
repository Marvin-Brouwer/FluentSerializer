using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Services
{
    public class JsonTypeSerializer
    {
        private readonly IScanList<(Type type, SerializerDirection direction), IClassMap> _mappings;

        public JsonTypeSerializer(IScanList<(Type type, SerializerDirection direction), IClassMap> mappings)
        {
            Guard.Against.Null(mappings, nameof(mappings));

            _mappings = mappings;
        }

        public JToken? SerializeToToken(object dataModel, Type classType, IJsonSerializer currentSerializer)
        {
            Guard.Against.Null(dataModel, nameof(dataModel));
            Guard.Against.Null(classType, nameof(classType));
            Guard.Against.Null(currentSerializer, nameof(currentSerializer));

            if (typeof(IEnumerable).IsAssignableFrom(classType)) throw new NotImplementedException("Todo");

            var classMap = _mappings.Scan((classType, SerializerDirection.Serialize));
            if (classMap is null) throw new ClassMapNotFoundException(classType);

            var newElement = new JObject();
            foreach(var property in classType.GetProperties())
            {
                var propertyMapping = classMap.PropertyMaps.Scan(property);
                if (propertyMapping is null) continue;
                if (propertyMapping.Direction == SerializerDirection.Deserialize) continue;

                var propertyValue = property.GetValue(dataModel);
                if (propertyValue is null) continue;

                var serializerContext = new SerializerContext(
                    propertyMapping.Property, classType, propertyMapping.NamingStrategy, 
                    currentSerializer,
                    classMap.PropertyMaps, _mappings);

                SerializeProperty(propertyValue, newElement, propertyMapping, currentSerializer, serializerContext);
            }

            return newElement;
        }

        private void SerializeProperty(
            object propertyValue, JContainer newElement, IPropertyMap propertyMapping,  
            IJsonSerializer currentSerializer, SerializerContext serializerContext)
        {
            if (typeof(JProperty).IsAssignableFrom(propertyMapping.ContainerType))
            {
                SerializeJProperty(propertyValue, propertyMapping, newElement, serializerContext, currentSerializer);
                return;
            }

            throw new ContainerNotSupportedException(propertyMapping.ContainerType);
        }

        private void SerializeJProperty(object propertyValue, IPropertyMap propertyMapping,
            JContainer newElement, SerializerContext serializerContext, IJsonSerializer currentSerializer)
        {
            var matchingConverter = propertyMapping.GetConverter<JToken>(
                SerializerDirection.Serialize, serializerContext.CurrentSerializer);

            var nodeValue = matchingConverter is null 
                ? SerializeToToken(propertyValue, serializerContext.PropertyType, currentSerializer) 
                : matchingConverter.Serialize(propertyValue, serializerContext);
            if (nodeValue is null) return;

            var propertyName = propertyMapping.NamingStrategy.GetName(propertyMapping.Property, serializerContext);
            newElement.Add(new JProperty(propertyName, nodeValue));
        }
    }
}
