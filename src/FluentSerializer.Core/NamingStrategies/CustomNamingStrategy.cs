using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public sealed class CustomNamingStrategy : INamingStrategy
    {
        private readonly string _name;

        public CustomNamingStrategy(string name)
        {
            _name = name;
        }

        public string GetName(PropertyInfo property) => _name;
    }
}
