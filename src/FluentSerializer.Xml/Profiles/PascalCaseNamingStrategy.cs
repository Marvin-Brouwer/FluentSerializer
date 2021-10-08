using System.Reflection;

namespace FluentSerializer.Xml.Profiles
{
    public class PascalCaseNamingStrategy : INamingStrategy
    {
        // Todo, see what JsonDotnet does here
        public virtual string GetName(PropertyInfo property) => property.Name;
    }
}
