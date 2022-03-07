using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

public static class ConverterExtensions
{
	private static readonly INamingStrategy RequestEntityNameStrategy = new RequestEntityNameStrategy();
	private static readonly INamingStrategy ReferenceNamingStrategy = new ReferenceNamingStrategy();
	private static readonly INamingStrategy ReferenceGroupNamingStrategy = new ReferenceGroupNamingStrategy();

	/// <inheritdoc cref="NamingStrategies.RequestEntityNameStrategy"/>
	public static INamingStrategy RequestEntityName(this IUseNamingStrategies _) => RequestEntityNameStrategy;

	/// <inheritdoc cref="NamingStrategies.ReferenceNamingStrategy"/>
	public static INamingStrategy ReferencePointer(this IUseNamingStrategies _) => ReferenceNamingStrategy;

	/// <inheritdoc cref="NamingStrategies.ReferenceGroupNamingStrategy"/>
	public static INamingStrategy ReferencesPointer(this IUseNamingStrategies _) => ReferenceGroupNamingStrategy;
}