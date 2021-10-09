using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public abstract class ClassMap : IClassMap
    {
        public ClassMap(
            Type classType,
            INamingStrategy namingStrategy,
            IEnumerable<IPropertyMap> propertyMap)
        {
            ClassType = classType;
            NamingStrategy = namingStrategy;
            PropertyMaps = propertyMap.ToList().AsReadOnly();
            PropertyMapLookup = propertyMap.ToLookup(x => x.Property, x => x);
        }
        public Type ClassType { get; }
        public INamingStrategy NamingStrategy { get; }

        public IReadOnlyCollection<IPropertyMap> PropertyMaps { get; }
        public ILookup<PropertyInfo, IPropertyMap> PropertyMapLookup { get; }
    }
}
