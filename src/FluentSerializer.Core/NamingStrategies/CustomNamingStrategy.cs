using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;
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

        public string GetName(PropertyInfo property, INamingContext _) => _name;

        public string GetName(Type classType, INamingContext _) => _name;
    }
}
