using Ardalis.GuardClauses;

using System;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Extensions;

internal static class EnumExtensions
{
	/// <summary>
	/// Throws an <see cref="ArgumentException" /> if  <paramref name="input"/> has a value of <paramref name="invalidChoice"/> function.
	/// </summary>
	/// <param name="_"></param>
	/// <param name="input"></param>
	/// <param name="parameterName"></param>
	/// <param name="invalidChoice"></param>
	/// <param name="message">Optional. Custom error message</param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static T InvalidChoice<T>(
		this IGuardClause _,
		T input,
		T invalidChoice,
		string? message = null,
		[CallerArgumentExpression("input")] string parameterName = null!)
		where T : Enum
	{
		if (input.Equals(invalidChoice))
			throw new ArgumentException(message ?? $"Input {parameterName} cannot have a value of {input}", parameterName);

		return input;
	}
}
