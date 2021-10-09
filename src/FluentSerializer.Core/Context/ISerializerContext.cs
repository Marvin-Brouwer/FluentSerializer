using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Core.Services;
using System.Reflection;

namespace FluentSerializer.Core.Context
{
    public interface ISerializerContext
    {
        PropertyInfo Property { get; }
        INamingStrategy NamingStrategy { get; }
        ISerializer CurrentSerializer { get; }
    }
}
