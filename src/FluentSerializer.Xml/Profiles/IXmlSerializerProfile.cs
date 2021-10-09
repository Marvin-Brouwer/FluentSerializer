using FluentSerializer.Xml.Profiles;
using System;
using System.Collections.Generic;

namespace FluentSerializer.Xml.Configuration
{
    public interface IXmlSerializerProfile
    {
        Dictionary<Type, XmlClassMap> Configure();
    }
}