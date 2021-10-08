using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public interface INamingStrategy
    {
        public string GetName(PropertyInfo property);
    }
}
