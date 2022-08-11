namespace FluentSerializer.Xml.Converting;

/// <summary>
/// XML converters selector
/// </summary>
public static class Converter
{
	/// <inheritdoc cref="IUseXmlConverters" />
	public static IUseXmlConverters For { get; } = new UseXmlConverters();
}