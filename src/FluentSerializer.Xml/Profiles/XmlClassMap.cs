using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class XmlClassMap : ClassMap
    {
        public XmlClassMap(
            Type classType, INamingStrategy namingStrategy, IEnumerable<XmlPropertyMap> propertyMap) : 
            base(classType, namingStrategy, propertyMap)
        { }
    }
}
