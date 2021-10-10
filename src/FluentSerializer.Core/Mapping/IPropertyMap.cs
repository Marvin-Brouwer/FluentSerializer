using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Mapping
{
    public interface IPropertyMap
    {
        IConverter? CustomConverter { get; }
        SerializerDirection Direction { get; }
        INamingStrategy NamingStrategy { get; }
        PropertyInfo Property { get; }
        Type ConcretePropertyType { get; }
        Type ContainerType { get; }
    }
}