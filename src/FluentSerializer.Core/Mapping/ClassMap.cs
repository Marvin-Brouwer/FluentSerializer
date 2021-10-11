using FluentSerializer.Core.NamingStrategies;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public sealed class ClassMap : IClassMap
    {
        public ClassMap(
            Type classType,
            INamingStrategy namingStrategy,
            IEnumerable<IPropertyMap> propertyMap)
        {
            ClassType = classType;
            NamingStrategy = namingStrategy;
            PropertyMaps = new PropertyMapScanList(propertyMap);
        }
        public Type ClassType { get; }
        public INamingStrategy NamingStrategy { get; }

        public IScanList<PropertyInfo, IPropertyMap> PropertyMaps { get; }
    }
}
