using System;
using System.Collections.Generic;

namespace FluentSerializer.Xml.Profiles
{
    public interface IXmlSerializerProfile
    {
        Dictionary<Type, XmlClassMap> Configure();
    }
}