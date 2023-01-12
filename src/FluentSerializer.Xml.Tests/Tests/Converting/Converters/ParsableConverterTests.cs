using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="IParsable{TSelf}"/>
/// </summary>
public sealed partial class ParsableConverterTests
{
	private readonly Mock<ISerializerContext<IXmlNode>> _contextMock;
	private static readonly CultureInfo TestCulture = new("nl-NL");

	private static IXmlConverter SutParse(IFormatProvider? formatProvider = null) => new ParsableConverter(false, formatProvider);
	private static IXmlConverter SutTryParse(IFormatProvider? formatProvider = null) => new ParsableConverter(true, formatProvider);

	public ParsableConverterTests()
	{
		var serializerMock = new Mock<IAdvancedXmlSerializer>()
			.UseConfig(XmlSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IXmlNode>>()
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
