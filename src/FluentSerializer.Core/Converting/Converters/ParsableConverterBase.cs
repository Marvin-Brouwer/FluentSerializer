#if NET7_0_OR_GREATER
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

using System;
using System.Globalization;
using System.Reflection;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts types that implement <see cref="IConvertible"/>
/// </summary>
public abstract class ParsableConverterBase : IConverter
{
	/// <inheritdoc cref="IConverter.Direction" />
	public SerializerDirection Direction { get; } = SerializerDirection.Deserialize;

	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public bool CanConvert(in Type targetType) => targetType.Implements(typeof(IParsable<>));

	/// <inheritdoc />
	public Guid ConverterId { get; } = typeof(IParsable<>).GUID;

	private readonly CultureInfo? _formatProvider;
	private readonly bool _tryParse;
	private readonly string _tryParseMethodName;
	private readonly string _parseMethodName;
	private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;

	private CultureInfo FormatProvider => _formatProvider ?? CultureInfo.CurrentCulture;

	/// <inheritdoc cref="ConvertibleConverterBase"/>
	protected ParsableConverterBase(in CultureInfo? formatProvider, in bool tryParse)
	{
		_formatProvider = formatProvider;
		_tryParse = tryParse;
		_tryParseMethodName = nameof(IParsable<int>.TryParse);
		_parseMethodName = nameof(IParsable<int>.Parse);
	}

	/// <summary>
	/// Wrapper around <see cref="IParsable{TSelf}"/> to support nullable values
	/// </summary>
	protected object? ConvertToNullableDataType(in string? currentValue, in Type targetType)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		if (_tryParse)
			return TryParse(currentValue, targetType);

		return Parse(currentValue, targetType);
	}

	private object? TryParse(string? currentValue, Type targetType)
	{
		var argTypes = new[] { typeof(string), typeof(IFormatProvider), targetType.MakeByRefType() };

		var parse = targetType.GetMethod(_tryParseMethodName, bindingFlags, argTypes);
		if (parse is null) throw new EntryPointNotFoundException($"The method '{_tryParseMethodName}' was not found on {nameof(IParsable<int>)} type");

		var parseArugments = new object?[] { currentValue, FormatProvider, null };
		var parseSuccess = (bool)parse.Invoke(null, parseArugments)!;
		if (!parseSuccess) return null;

		return parseArugments[2];
	}

	private object? Parse(string? currentValue, Type targetType)
	{
		var argTypes = new[] { typeof(string), typeof(IFormatProvider) };

		var parse = targetType.GetMethod(_parseMethodName, bindingFlags, argTypes);
		if (parse is null) throw new EntryPointNotFoundException($"The method '{_parseMethodName}' was not found on {nameof(IParsable<int>)} type");

		try
		{
			var parseArugments = new object?[] { currentValue, FormatProvider };
			return parse.Invoke(null, parseArugments)!;
		}
		catch(TargetInvocationException invocationException)
		{
			throw invocationException.InnerException!;
		}
	}
}
#endif