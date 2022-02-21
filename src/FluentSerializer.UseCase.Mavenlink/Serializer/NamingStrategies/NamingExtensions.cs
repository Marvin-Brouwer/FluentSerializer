using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

public static class ConverterExtensions
{
	private static readonly INamingStrategy RequestEntityNameStrategy = new RequestEntityNameStrategy();

	/// <inheritdoc cref="NamingStrategies.RequestEntityNameStrategy"/>
	public static INamingStrategy RequestEntityName(this IUseNamingStrategies _) => RequestEntityNameStrategy;
}