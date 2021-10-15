using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Mapping
{
    public sealed class ClassMap : IClassMap
    {
        private readonly Func<INamingStrategy> _namingStrategy;

        public Type ClassType { get; }
        public INamingStrategy NamingStrategy => _namingStrategy();
        public IScanList<PropertyInfo, IPropertyMap> PropertyMaps { get; }

        public ClassMap(
            Type classType,
            Func<INamingStrategy> namingStrategy,
            IEnumerable<IPropertyMap> propertyMap)
        {
            ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
            _namingStrategy = namingStrategy;
            PropertyMaps = new PropertyMapScanList(propertyMap);
        }
    }
}
