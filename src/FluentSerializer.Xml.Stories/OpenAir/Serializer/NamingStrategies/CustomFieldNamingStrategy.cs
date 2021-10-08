using FluentSerializer.Xml.Profiles;
using System.Reflection;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public class CustomFieldNamingStrategy : SnakeCaseNamingStrategy
    {
        public override string GetName(PropertyInfo property) => $"{base.GetName(property)}__c";
    }
}