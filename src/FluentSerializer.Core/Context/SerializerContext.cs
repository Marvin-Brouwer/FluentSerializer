using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Context
{
    /// <inheritdoc cref="ISerializerContext"/>
    public sealed class SerializerContext : ISerializerContext
    {
        private readonly IScanList<PropertyInfo, IPropertyMap> _propertyMappings;
        private readonly IScanList<Type, IClassMap> _classMappings;

        public PropertyInfo Property { get; }

        public Type PropertyType { get; }

        public Type ClassType { get; }

        public INamingStrategy NamingStrategy  { get; }

        public ISerializer CurrentSerializer  { get; }

        public SerializerContext(
            PropertyInfo property, Type classType, 
            INamingStrategy namingStrategy, ISerializer currentSerializer,
            IScanList<PropertyInfo, IPropertyMap> propertyMappings,
            IScanList<Type, IClassMap> classMappings)
        {
            _propertyMappings = propertyMappings;
            _classMappings = classMappings;

            var realPropertyType = classType.GetProperty(property.Name)!.PropertyType;

            Property = property;
            PropertyType = Nullable.GetUnderlyingType(realPropertyType) ?? realPropertyType;
            ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
            NamingStrategy = namingStrategy;
            CurrentSerializer = currentSerializer;
        }

        public INamingStrategy? FindNamingStrategy(PropertyInfo property) => 
            _propertyMappings.Scan(property)?.NamingStrategy;

        public INamingStrategy? FindNamingStrategy(Type type) => 
            _classMappings.Scan(type)?.NamingStrategy;
    }
}
