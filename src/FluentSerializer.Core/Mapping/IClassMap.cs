using FluentSerializer.Core.NamingStrategies;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public interface IClassMap
    {
        INamingStrategy NamingStrategy { get; }
        Type ClassType { get; }

        IScanList<PropertyInfo, IPropertyMap> PropertyMaps { get; }
    }
}