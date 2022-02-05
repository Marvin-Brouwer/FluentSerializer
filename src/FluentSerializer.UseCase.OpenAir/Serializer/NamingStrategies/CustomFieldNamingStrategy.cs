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
        
        public CustomFieldNamingStrategy(in string name)
        {
            _innerNamingStrategy = Names.Are(name)();
        }

        public CustomFieldNamingStrategy()
        {
            _innerNamingStrategy = Names.Use.SnakeCase();
        }
        public string GetName(in PropertyInfo property, in INamingContext namingContext) => GetName(_innerNamingStrategy.GetName(in property, in namingContext));
        public string GetName(in Type classType, in INamingContext namingContext) => GetName(_innerNamingStrategy.GetName(in classType, in namingContext));

        private string GetName(in string name) => $"{name}__c";
    }
}