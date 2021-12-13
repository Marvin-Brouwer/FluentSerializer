using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Naming.NamingStrategies
{
    public sealed class CustomNamingStrategy : INamingStrategy
    {
        private readonly string _name;

        public CustomNamingStrategy(string name)
        {
            Guard.Against.InvalidName(name, nameof(name));

            _name = name;
        }

        public string GetName(PropertyInfo property, INamingContext namingContext) => _name;

        public string GetName(Type classType, INamingContext namingContext) => _name;
    }
}
