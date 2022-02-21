using System;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;

public static class ConverterExtensions
{
	private static readonly INamingStrategy DefaultCustomNamingStrategy = new CustomFieldNamingStrategy();
	private static readonly INamingStrategy ResponseTypeNamingStrategy = new ResponseTypeNamingStrategy();

	/// <inheritdoc cref="NamingStrategies.CustomFieldNamingStrategy()"/>
	public static INamingStrategy CustomFieldName(this IUseNamingStrategies _) => DefaultCustomNamingStrategy;
	/// <inheritdoc cref="NamingStrategies.CustomFieldNamingStrategy(in string)"/>
	public static Func<INamingStrategy> CustomFieldName(this IUseNamingStrategies _, string name) => () => new CustomFieldNamingStrategy(name);

	/// <inheritdoc cref="NamingStrategies.ResponseTypeNamingStrategy"/>
	public static INamingStrategy ResponseTypeName (this IUseNamingStrategies _) => ResponseTypeNamingStrategy;
}