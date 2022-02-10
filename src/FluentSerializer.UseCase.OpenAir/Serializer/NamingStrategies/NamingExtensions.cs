using System;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies
{
    public static class ConverterExtensions
    {
	    private static readonly INamingStrategy DefaultCustomNamingStrategy = new CustomFieldNamingStrategy();
	    private static readonly INamingStrategy ResponseTypeNamingStrategy = new ResponseTypeNamingStrategy();

	    public static INamingStrategy CustomFieldName(this IUseNamingStrategies _) => DefaultCustomNamingStrategy;
        public static Func<INamingStrategy> CustomFieldName(this IUseNamingStrategies _, string name) => () => new CustomFieldNamingStrategy(name);
        public static INamingStrategy ResponseTypeName (this IUseNamingStrategies _) => ResponseTypeNamingStrategy;
    }
}
