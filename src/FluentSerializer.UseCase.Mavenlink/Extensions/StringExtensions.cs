using System.Collections.Generic;

namespace FluentSerializer.UseCase.Mavenlink.Extensions;

internal static class StringExtensions
{
	/// <inheritdoc cref="string.Join{T}(char, IEnumerable{T})"/>
	internal static string Join(this IEnumerable<string> stringToJoin, string separator)
	{
		return string.Join(separator, stringToJoin);
	}
}