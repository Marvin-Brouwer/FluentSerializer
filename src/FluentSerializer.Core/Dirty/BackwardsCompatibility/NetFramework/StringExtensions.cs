#if NETSTANDARD2_0

namespace FluentSerializer.Core.Dirty.BackwardsCompatibility.NetFramework;

/// <summary>
/// Extensions for backward compatibility
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Determines whether the beginning of this string instance matches the specified string.
	/// </summary>
	/// <param name="sourceString"></param>
	/// <param name="value">The string to compare.</param>
	/// <returns>true if value matches the beginning of this string; otherwise, false.</returns>
	/// <exception cref="System.ArgumentNullException">value is null.</exception>
	public static bool StartsWith(this string sourceString, char value) =>
		sourceString.StartsWith(value.ToString(System.Globalization.CultureInfo.InvariantCulture), System.StringComparison.Ordinal);
}
#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(string))]
#endif