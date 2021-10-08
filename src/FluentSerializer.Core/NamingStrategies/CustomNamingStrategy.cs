using System.Reflection;

namespace FluentSerializer.Xml.Profiles
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
