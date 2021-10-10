using FluentSerializer.Core.NamingStrategies;
using FluentSerializer.Xml.Stories.OpenAir.Models.Request;
using System;
using System.Reflection;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public class ResponseTypeNamingStrategy : INamingStrategy
    {
        public string GetName(PropertyInfo property) => GetName(property.DeclaringType!);

        public string GetName(Type classType)
        {
            if (!typeof(RequestObject<>).IsAssignableFrom(classType.GetGenericTypeDefinition()))
                throw new NotSupportedException("This strategy is only meant for RequestObjects");

            var genericType = classType.GetTypeInfo().GenericTypeArguments[0];
            return "";
        }
    }
}