using System.Collections.Generic;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.Profiles
{
    public interface ISerializerProfile
    {
        IEnumerable<IClassMap> Configure();
    }
}