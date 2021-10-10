using FluentSerializer.Xml.Mapping;
using System.Collections.Generic;

namespace FluentSerializer.Xml.Profiles
{
    // Todo this is possibly a candidate for ISerializerProfile<TClassMap> to keep consistency
    public interface IXmlSerializerProfile
    {
        IEnumerable<XmlClassMap> Configure();
    }
}