using FluentSerializer.Core.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies
{
    public class CustomFieldNamingStrategy : SnakeCaseNamingStrategy
    {
        protected override string GetName(string name) => $"{base.GetName(name)}__c";
    }
}