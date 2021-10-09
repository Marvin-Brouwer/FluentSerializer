using System;
using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public interface INamingStrategy
    {
        public string GetName(PropertyInfo property);
        public string GetName(Type classType);
    }
}
