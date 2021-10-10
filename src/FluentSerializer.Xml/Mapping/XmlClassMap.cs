using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.NamingStrategies;
using System;
using System.Collections.Generic;

namespace FluentSerializer.Xml.Mapping
{
    public sealed class XmlClassMap : ClassMap
    {
        public XmlClassMap((Type classType, INamingStrategy namingStrategy, IEnumerable<IPropertyMap> propertyMap) lazyClassMap) :
            base(lazyClassMap.classType, lazyClassMap.namingStrategy, lazyClassMap.propertyMap)
        {
        }
    }
}
