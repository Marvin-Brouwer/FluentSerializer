using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Factory;
using FluentSerializer.Json.Profiles;

using Microsoft.Extensions.ObjectPool;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

namespace FluentSerializer.Json.Tests.Tests.Factories;

/// <remarks>
/// These tests are purely to verify that the <see cref="JsonSerializerFactory"/> does everything
/// the <see cref="BaseSerializerFactory{TSerializer,TConfiguration,TSerializerProfile}"/> fully.
/// Not all assertions will make sense.
/// </remarks>
public sealed class JsonSerializerFactoryTests
{
	private readonly JsonSerializerFactory _sut;

	public JsonSerializerFactoryTests()
	{
		_sut = new();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void WithConfiguration_UseDefaults_ReturnsWithDefaults()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(Array.Empty<JsonSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().BeEquivalentTo(JsonSerializerConfiguration.Default);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void WithConfiguration_SkipCall_ReturnsWithDefaults()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.UseProfiles(Array.Empty<JsonSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().BeEquivalentTo(JsonSerializerConfiguration.Default);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void WithConfiguration_SetOverrides_ReturnsWithOverrides()
	{
		// Arrange
		var config = new JsonSerializerConfiguration
		{
			NewLine = "TEST"
		};
		var poolProvider = new DefaultObjectPoolProvider();

		// Act
		var resultWithSetConfiguration = _sut
			.WithConfiguration(config, poolProvider)
			.UseProfiles(Array.Empty<JsonSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().NotBe(JsonSerializerConfiguration.Default);
		resultWithSetConfiguration.Configuration.Should().NotBeEquivalentTo(new TestConfiguration());
		resultWithSetConfiguration.Configuration.NewLine.Should().Be("TEST");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void WithConfiguration_ConfigureWithAction_ReturnsWithOverrides()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithConfiguration(config =>
			{
				config.NewLine = "TEST";
			})
			.UseProfiles(Array.Empty<JsonSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().NotBe(JsonSerializerConfiguration.Default);
		resultWithSetConfiguration.Configuration.Should().NotBeEquivalentTo(new TestConfiguration());
		resultWithSetConfiguration.Configuration.NewLine.Should().Be("TEST");
	}

	/// <remarks>
	/// None of these scenarios should ever happen.
	/// So, we should test it throws an exception.
	/// </remarks>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void WithConfiguration_NullConfiguration_Throws()
	{
		// Arrange
		var configuration = (JsonSerializerConfiguration)null!;
		var configurationSetup = (Action<JsonSerializerConfiguration>)null!;

		// Act
		var result1 = () => _sut
			.WithConfiguration(configuration);
		var result2 = () => _sut
			.WithConfiguration(configurationSetup);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(configuration));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(configurationSetup));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseProfile_AddProfile_ReturnsWithSingleProfile()
	{
		// Arrange
		var config = new TestSerializerProfile();

		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfile(config);

		// Assert (Just assert it still works)
		resultWithSetConfiguration.Should().NotBeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseProfile_AddProfiles_AddOne_ReturnsWithSingleProfile()
	{
		// Arrange
		var config = new TestSerializerProfile();

		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(new List<JsonSerializerProfile> { config });

		// Assert (Just assert it still works)
		resultWithSetConfiguration.Should().NotBeNull();
	}

	/// <remarks>
	/// This would be considered invalid state, however we don't want to break on this.
	/// Theoretically if you use the assembly scanning version you may end up with 0 profiles temporarily during refactoring.
	/// </remarks>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void UseProfile_AddProfiles_AddNone_ReturnsWithoutProfiles()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(Array.Empty<JsonSerializerProfile>());

		// Assert (Just assert it still works)
		resultWithSetConfiguration.Should().NotBeNull();
	}

	private sealed class TestConfiguration : SerializerConfiguration { }

	private sealed class TestSerializerProfile : JsonSerializerProfile, ISerializerProfile<JsonSerializerConfiguration>
	{
		protected override void Configure() => throw new NotSupportedException("Out of test scope");

		IReadOnlyCollection<IClassMap> ISerializerProfile<JsonSerializerConfiguration>.Configure(in JsonSerializerConfiguration configuration) => new List<IClassMap>
		{
			Mock.Of<IClassMap>(MockBehavior.Loose)
		};
	}
}
