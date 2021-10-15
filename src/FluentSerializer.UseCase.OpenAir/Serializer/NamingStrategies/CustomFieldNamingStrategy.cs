using System;
using System.Reflection;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies
{
    public sealed class CustomFieldNamingStrategy : INamingStrategy
    {
        private readonly INamingStrategy _innerNamingStrategy;
        
        public CustomFieldNamingStrategy(string name)
        {
            _innerNamingStrategy = Names.Are(name)();
        }

        public CustomFieldNamingStrategy()
        {
            _innerNamingStrategy = Names.Use.SnakeCase();
        }
        public string GetName(PropertyInfo property, INamingContext namingContext) => GetName(_innerNamingStrategy.GetName(property, namingContext));
        public string GetName(Type classType, INamingContext namingContext) => GetName(_innerNamingStrategy.GetName(classType, namingContext));

        private string GetName(string name) => $"{name}__c";
    }
}