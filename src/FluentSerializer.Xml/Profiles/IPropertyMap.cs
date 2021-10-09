using System;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface IPropertyMap<out TDestination>
        where TDestination : class
    {
        IConverter? CustomConverter { get; }
        SerializerDirection Direction { get; }
        INamingStrategy NamingStrategy { get; }
        PropertyInfo Property { get; }
        Type DestinationType { get; }
    }
}