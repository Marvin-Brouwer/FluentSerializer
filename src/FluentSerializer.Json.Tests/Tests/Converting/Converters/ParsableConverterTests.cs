using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.Tostring"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed partial class ParsableConverterTests
{
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;
	private static readonly CultureInfo TestCulture = new ("nl-NL");

	private static ParsableConverter SutParse(CultureInfo? format = null) => new ParsableConverter(format, false);
	private static ParsableConverter SutTryParse(CultureInfo? format = null) => new ParsableConverter(format, true);

	public ParsableConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { new DateOnly(1991, 11, 28), "28-11-1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), "12:00:00" };
		yield return new object[] { 6.9, "6,9" };
	}
}
