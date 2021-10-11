using Ardalis.GuardClauses;
using System;
using System.Reflection;

namespace FluentSerializer.Core.NamingStrategies
{
    public sealed class CustomNamingStrategy : INamingStrategy
    {
        private readonly string _name;

        public CustomNamingStrategy(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.InvalidFormat(name, nameof(name), @"^[\w_\-+]*$");

            _name = name;
        }

        public string GetName(PropertyInfo property) => _name;

        public string GetName(Type classType) => _name;
    }
}
