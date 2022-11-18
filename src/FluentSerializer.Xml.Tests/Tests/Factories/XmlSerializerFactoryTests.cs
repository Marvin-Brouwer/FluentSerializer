using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Factory;
using FluentSerializer.Xml.Profiles;

using Microsoft.Extensions.ObjectPool;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests.Factories;

/// <remarks>
/// These tests are purely to verify that the <see cref="XmlSerializerFactory"/> does everything
/// the <see cref="BaseSerializerFactory{TSerializer,TConfiguration,TSerializerProfile}"/> fully.
/// Not all assertions will make sense.
/// </remarks>
public sealed class XmlSerializerFactoryTests
{
	private readonly XmlSerializerFactory _sut;

	public XmlSerializerFactoryTests()
	{
		_sut = new();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void WithConfiguration_UseDefaults_ReturnsWithDefaults()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(Array.Empty<XmlSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().BeEquivalentTo(XmlSerializerConfiguration.Default);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void WithConfiguration_SkipCall_ReturnsWithDefaults()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.UseProfiles(Array.Empty<XmlSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().BeEquivalentTo(XmlSerializerConfiguration.Default);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void WithConfiguration_SetOverrides_ReturnsWithOverrides()
	{
		// Arrange
		var config = new XmlSerializerConfiguration
		{
			NewLine = "TEST"
		};
		var poolProvider = new DefaultObjectPoolProvider();

		// Act
		var resultWithSetConfiguration = _sut
			.WithConfiguration(config, poolProvider)
			.UseProfiles(Array.Empty<XmlSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().NotBe(XmlSerializerConfiguration.Default);
		resultWithSetConfiguration.Configuration.Should().NotBeEquivalentTo(new TestConfiguration());
		resultWithSetConfiguration.Configuration.NewLine.Should().Be("TEST");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void WithConfiguration_ConfigureWithAction_ReturnsWithOverrides()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithConfiguration(config =>
			{
				config.NewLine = "TEST";
			})
			.UseProfiles(Array.Empty<XmlSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().NotBe(XmlSerializerConfiguration.Default);
		resultWithSetConfiguration.Configuration.Should().NotBeEquivalentTo(new TestConfiguration());
		resultWithSetConfiguration.Configuration.NewLine.Should().Be("TEST");
	}

	/// <remarks>
	/// None of these scenarios should ever happen.
	/// So, we should test it throws an exception.
	/// </remarks>

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void WithConfiguration_NullConfiguration_Throws()
	{
		// Arrange
		var configuration = (XmlSerializerConfiguration)null!;
		var configurationSetup = (Action<XmlSerializerConfiguration>)null!;

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
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
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
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void UseProfile_AddProfiles_AddOne_ReturnsWithSingleProfile()
	{
		// Arrange
		var config = new TestSerializerProfile();

		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(new List<XmlSerializerProfile> { config });

		// Assert (Just assert it still works)
		resultWithSetConfiguration.Should().NotBeNull();
	}

	/// <remarks>
	/// This would be considered invalid state, however we don't want to break on this.
	/// Theoretically if you use the assembly scanning version you may end up with 0 profiles temporarily during refactoring.
	/// </remarks>

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void UseProfile_AddProfiles_AddNone_ReturnsWithoutProfiles()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(Array.Empty<XmlSerializerProfile>());

		// Assert (Just assert it still works)
		resultWithSetConfiguration.Should().NotBeNull();
	}

	private sealed class TestConfiguration: SerializerConfiguration { }

	private sealed class TestSerializerProfile : XmlSerializerProfile, ISerializerProfile<XmlSerializerConfiguration>
	{
		protected override void Configure() => throw new NotSupportedException("Out of test scope");

		IReadOnlyCollection<IClassMap> ISerializerProfile<XmlSerializerConfiguration>.Configure(in XmlSerializerConfiguration configuration) => new List<IClassMap>
		{
			Mock.Of<IClassMap>(MockBehavior.Loose)
		};
	}
}
