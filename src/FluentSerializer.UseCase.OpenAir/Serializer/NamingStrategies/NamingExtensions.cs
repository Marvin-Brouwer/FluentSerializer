using System;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies
{
    public static class ConverterExtensions
    {
        public static INamingStrategy CustomFieldName(this IUseNamingStrategies _) => new CustomFieldNamingStrategy();
        public static Func<INamingStrategy> CustomFieldName(this IUseNamingStrategies _, string name) => () => new CustomFieldNamingStrategy(name);
        public static INamingStrategy ResponseTypeName (this IUseNamingStrategies _) => new ResponseTypeNamingStrategy();
    }
}
