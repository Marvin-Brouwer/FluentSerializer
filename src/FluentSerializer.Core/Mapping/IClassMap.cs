using FluentSerializer.Core.NamingStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public interface IClassMap
    {
        INamingStrategy NamingStrategy { get; }
        Type ClassType { get; }

        IReadOnlyCollection<IPropertyMap> PropertyMaps { get; }
        ILookup<PropertyInfo, IPropertyMap> PropertyMapLookup { get; }
    }
}