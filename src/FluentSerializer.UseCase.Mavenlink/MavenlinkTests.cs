using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.Converter.DefaultJson.Extensions;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Extensions;
using FluentSerializer.Json.Services;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace FluentSerializer.UseCase.Mavenlink;

public sealed partial class MavenlinkTests
{
	private readonly IJsonSerializer _sut;

	public MavenlinkTests()
	{
		_sut = SerializerFactory.For
			.Json(static configuration =>
			{
				configuration.DefaultNamingStrategy = Names.Use.SnakeCase;
				configuration.DefaultConverters.Use(Converter.For.Json());
				configuration.NewLine = LineEndings.LineFeed;
			})
			.UseProfilesFromAssembly<MavenlinkTests>();
	}

	[Fact,
		Trait("Category", "UseCase")]
	public async Task Serialize()
	{
		// Arrange
		var expected = await File.ReadAllTextAsync("./MavenlinkTests.Serialize.json");
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
		var expected = UserResponseExample;
		var example = await File.ReadAllTextAsync("./MavenlinkTests.Deserialize.json");

		// Act
		var result = _sut.Deserialize<Response<User>>(example);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private static DateTime CreateDate(string dateString) => DateTime.ParseExact(
		dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
}