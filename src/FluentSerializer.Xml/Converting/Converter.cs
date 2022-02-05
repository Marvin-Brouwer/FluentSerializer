namespace FluentSerializer.Xml.Converting;

/// <summary>
/// XML converters selector
/// </summary>
public readonly struct Converter
{
	/// <inheritdoc cref="IUseXmlConverters" />
	public static IUseXmlConverters For { get; } = new UseXmlConverters();
}