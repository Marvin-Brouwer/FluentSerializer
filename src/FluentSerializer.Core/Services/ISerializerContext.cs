using FluentSerializer.Core;
using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface ISerializerContext
    {
        PropertyInfo Property { get; }
        INamingStrategy NamingStrategy { get; }
        ISerializer CurrentSerializer { get; }
    }
}
