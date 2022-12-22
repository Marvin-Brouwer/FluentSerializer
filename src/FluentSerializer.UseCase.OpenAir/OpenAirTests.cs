using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.UseCase.OpenAir.Models;
using FluentSerializer.UseCase.OpenAir.Models.Response;
using FluentSerializer.Xml.Converter.DefaultXml.Converting.Extensions;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Extensions;
using FluentSerializer.Xml.Services;

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace FluentSerializer.UseCase.OpenAir;

public sealed partial class OpenAirTests
{
	private readonly IXmlSerializer _sut;

	public OpenAirTests()
	{
		_sut = SerializerFactory.For
			.Xml(static configuration =>
			{
				configuration.Encoding = Encoding.UTF8;
				configuration.DefaultPropertyNamingStrategy = Names.Use.SnakeCase;
				configuration.DefaultConverters.Use(Converter.For.Xml());
				configuration.NewLine = LineEndings.LineFeed;
			})
			.UseProfilesFromAssembly<OpenAirTests>();
	}

	[Fact,
		Trait("Category", "UseCase")]
	public async Task Serialize()
	{
		// Arrange
		var expected = await File.ReadAllTextAsync("./OpenAirTests.Serialize.xml");
		var example = ProjectRequestExample;

		// Act
		var result = _sut.Serialize(example);

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UseCase")]
	public async Task Deserialize()
	{
		// Arrange
		var expected = RateCardResponseExample;
		var example = await File.ReadAllTextAsync("./OpenAirTests.Deserialize.xml");

		// Act
		var result = _sut.Deserialize<Response<RateCard>>(example);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private static DateTime CreateDate(string dateString) => DateTime.ParseExact(
		dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
}