using FluentSerializer.Core.NamingStrategies;
using System.Reflection;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.NamingStrategies
{
    public class CustomFieldNamingStrategy : SnakeCaseNamingStrategy
    {
        public override string GetName(PropertyInfo property) => $"{base.GetName(property)}__c";
    }
}