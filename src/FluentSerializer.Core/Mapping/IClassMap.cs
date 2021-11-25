using System;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Mapping
{
    public interface IClassMap
    {
        INamingStrategy NamingStrategy { get; }
        Type ClassType { get; }

        IScanList<PropertyInfo, IPropertyMap> PropertyMaps { get; }
        SerializerDirection Direction { get; }
    }
}