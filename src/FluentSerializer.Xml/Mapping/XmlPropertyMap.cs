using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;

namespace FluentSerializer.Xml.Mapping
{
    public sealed class XmlPropertyMap : PropertyMap
    {
        public XmlPropertyMap(
            SerializerDirection direction, Type containerType, PropertyInfo property, INamingStrategy namingStrategy, IConverter? customConverter) : 
            base (direction, containerType, property, namingStrategy, customConverter)
        { }
    }
}
