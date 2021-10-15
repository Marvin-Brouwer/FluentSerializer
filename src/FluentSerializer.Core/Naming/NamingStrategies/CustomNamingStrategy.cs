using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies
{
    public sealed class CustomNamingStrategy : INamingStrategy
    {
        private readonly string _name;

        public CustomNamingStrategy(string name)
        {
            // TODO put in extentionMethod and validate on use too
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.InvalidFormat(name, nameof(name), @"^[\w_\-+]*$");

            _name = name;
        }

        public string GetName(PropertyInfo property, INamingContext namingContext) => _name;

        public string GetName(Type classType, INamingContext namingContext) => _name;
    }
}
