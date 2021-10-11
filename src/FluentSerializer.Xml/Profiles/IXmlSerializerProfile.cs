using FluentSerializer.Core.Mapping;
using System.Collections.Generic;

namespace FluentSerializer.Xml.Profiles
{
    // Todo this is possibly a candidate for ISerializerProfile<TClassMap> to keep consistency
    public interface IXmlSerializerProfile
    {
        IEnumerable<IClassMap> Configure();
    }
}