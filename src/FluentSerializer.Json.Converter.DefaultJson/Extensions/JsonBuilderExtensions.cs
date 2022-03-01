namespace FluentSerializer.Json.Converter.DefaultJson.Extensions;

/// <summary>
/// Extensions to help building JSON data structures
/// </summary>
public static class JsonBuilderExtensions
{
	private static readonly string StringWrapper = '"'.ToString();

	/// <summary>
	/// Wrap a string value in quotes
	/// </summary>
	public static string WrapString(this string value) => string.Concat(
		StringWrapper, value, StringWrapper);
}