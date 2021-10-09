using System;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface IClassMap<TDestination, TPropertyDestination, TPropertyMap>
        where TDestination : class
        where TPropertyDestination : class
        where TPropertyMap : class, IPropertyMap<TPropertyDestination>
    {
        IConverter<TDestination>? CustomConverter { get; }
        INamingStrategy NamingStrategy { get; }
        Type ClassType { get; }

        ILookup<PropertyInfo, TPropertyMap> PropertyMap { get; }
    }
}