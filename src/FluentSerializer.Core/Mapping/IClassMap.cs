using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface IClassMap
    {
        INamingStrategy NamingStrategy { get; }
        Type ClassType { get; }

        IReadOnlyCollection<IPropertyMap> PropertyMaps { get; }
        ILookup<PropertyInfo, IPropertyMap> PropertyMapLookup { get; }
    }
}