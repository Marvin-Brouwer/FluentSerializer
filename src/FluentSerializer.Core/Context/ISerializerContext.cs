using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Context
{
    public interface ISerializerContext
    {
        PropertyInfo Property { get; }
        Type ClassType { get; }
        INamingStrategy NamingStrategy { get; }
        ISearchDictionary<Type, IClassMap> ClassMaps { get; }
        ISerializer CurrentSerializer { get; }
    }
}
