namespace FluentSerializer.Json.Converting;

/// <summary>
/// JSON converters selector
/// </summary>
public readonly struct Converter
{
	/// <inheritdoc cref="IUseJsonConverters" />
	public static IUseJsonConverters For { get; } = new UseJsonConverters();
}