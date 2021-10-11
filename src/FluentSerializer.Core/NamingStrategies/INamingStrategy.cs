using FluentSerializer.Core.Context;
using System;
using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public interface INamingStrategy
    {
        public string GetName(PropertyInfo property, INamingContext namingContext);
        public string GetName(Type classType, INamingContext namingContext);
    }
}
