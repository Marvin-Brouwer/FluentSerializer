using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Mapping
{
    public sealed class ClassMap : IClassMap
    {
        private readonly Func<INamingStrategy> _namingStrategy;
        private readonly IEnumerable<IPropertyMap> _propertyMap;

        public INamingStrategy NamingStrategy => _namingStrategy();
        public IScanList<PropertyInfo, IPropertyMap> PropertyMaps => new PropertyMapScanList(_propertyMap);

        public Type ClassType { get; }
        public SerializerDirection Direction { get; }

        public ClassMap(
            Type classType,
            SerializerDirection direction,
            Func<INamingStrategy> namingStrategy,
            IEnumerable<IPropertyMap> propertyMap)
        {
            _namingStrategy = namingStrategy;
            _propertyMap = propertyMap;

            Direction = direction;
            ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
        }

    }
}
