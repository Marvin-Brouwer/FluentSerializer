using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface IPropertyMap
    {
        IConverter? CustomConverter { get; }
        SerializerDirection Direction { get; }
        INamingStrategy NamingStrategy { get; }
        PropertyInfo Property { get; }
        Type DestinationType { get; }
    }
}