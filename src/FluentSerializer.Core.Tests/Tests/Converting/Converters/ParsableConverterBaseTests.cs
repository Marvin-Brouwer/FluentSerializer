using FluentSerializer.Core.Converting.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.ToString(bool)"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed partial class ParsableConverterBaseTests
{
	public ParsableConverterBaseTests()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL", useUserOverride: false);
	}

	public static IEnumerable<object[]> GenerateParsableData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { new DateOnly(1991, 11, 28), "28-11-1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), "12:00:00" };
		yield return new object[] { 6.9, "6,9" };
	}

	/// <inheritdoc cref="ParsableConverterBase"/>
	private sealed class Sut : ParsableConverterBase
	{
		public static Sut Parse => new(false);
		public static Sut TryParse => new(true);

		private Sut(bool tryParse) : base(tryParse, null) { }

		/// <inheritdoc cref="ParsableConverterBase.ConvertToNullableDataType"/>
		public new object? ConvertToNullableDataType(in string? currentValue, in Type targetType) =>
			base.ConvertToNullableDataType(in currentValue, in targetType);
	}
}
