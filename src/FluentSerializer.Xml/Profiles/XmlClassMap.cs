using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public sealed class XmlClassMap : ClassMap<XElement, XObject, XmlPropertyMap>
    {
        public XmlClassMap(
            Type classType, INamingStrategy namingStrategy, IConverter<XElement>? customConverter, List<XmlPropertyMap> propertyMap) : 
            base(classType, namingStrategy, customConverter, propertyMap)
        { }
    }
}
