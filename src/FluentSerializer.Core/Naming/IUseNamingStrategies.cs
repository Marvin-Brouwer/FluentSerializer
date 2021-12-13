
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming
{
    public interface IUseNamingStrategies
    {
        INamingStrategy CamelCase();
        INamingStrategy LowerCase();
        INamingStrategy PascalCase();
        INamingStrategy SnakeCase();
        INamingStrategy KebabCase();
    }
}