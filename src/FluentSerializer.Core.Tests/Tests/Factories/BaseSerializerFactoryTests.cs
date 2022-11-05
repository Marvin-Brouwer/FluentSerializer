using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Constants;
using FluentSerializer.Core.Factories;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Services;

using Microsoft.Extensions.ObjectPool;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Factories;

public sealed class BaseSerializerFactoryTests
{
	private readonly TestSerializerFactory _sut;

	public BaseSerializerFactoryTests()
	{
		_sut = new();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithConfiguration_UseDefaults_ReturnsWithDefaults()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(Array.Empty<TestSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().BeEquivalentTo(_sut.GetDefaultConfiguration());
		resultWithSetConfiguration.PoolProvider.Should().Be(FactoryConstants.DefaultObjectPoolProvider);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithConfiguration_SkipCall_ReturnsWithDefaults()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.UseProfiles(Array.Empty<TestSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().BeEquivalentTo(_sut.GetDefaultConfiguration());
		resultWithSetConfiguration.PoolProvider.Should().Be(FactoryConstants.DefaultObjectPoolProvider);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithConfiguration_SetOverrides_ReturnsWithOverrides()
	{
		// Arrange
		var config = new TestConfiguration();
		var poolProvider = new DefaultObjectPoolProvider();

		// Act
		var resultWithSetConfiguration = _sut
			.WithConfiguration(config, poolProvider)
			.UseProfiles(Array.Empty<TestSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().Be(config);
		resultWithSetConfiguration.PoolProvider.Should().Be(poolProvider);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithConfiguration_ConfigureWithAction_ReturnsWithOverrides()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithConfiguration(config =>
			{
				config.NewLine = "TEST";
			})
			.UseProfiles(Array.Empty<TestSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Configuration.Should().NotBe(_sut.GetDefaultConfiguration());
		resultWithSetConfiguration.Configuration.Should().NotBeEquivalentTo(new TestConfiguration());
		resultWithSetConfiguration.Configuration.NewLine.Should().Be("TEST");
	}

	/// <remarks>
	/// None of these scenarios should ever happen.
	/// So, we should test it throws an exception.
	/// </remarks>
	[Fact,
		Trait("Category", "UnitTest")]
	public void WithConfiguration_NullConfiguration_Throws()
	{
		// Act
		var result1 = () => _sut
			.WithConfiguration((TestConfiguration)null!);
		var result2 = () => _sut
			.WithConfiguration((Action<TestConfiguration>)null!);

		// Assert
		result1.Should().ThrowExactly<ArgumentNullException>();
		result2.Should().ThrowExactly<ArgumentNullException>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void UseProfile_AddProfile_ReturnsWithSingleProfile()
	{
		// Arrange
		var config = new TestSerializerProfile();

		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfile(config);

		// Assert
		resultWithSetConfiguration.Mappings.Should().HaveCount(1);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void UseProfile_AddProfiles_AddOne_ReturnsWithSingleProfile()
	{
		// Arrange
		var config = new TestSerializerProfile();

		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(new List<TestSerializerProfile> { config });

		// Assert
		resultWithSetConfiguration.Mappings.Should().HaveCount(1);
	}

	/// <remarks>
	/// This would be considered invalid state, however we don't want to break on this.
	/// Theoretically if you use the assembly scanning version you may end up with 0 profiles temporarily during refactoring.
	/// </remarks>
	[Fact,
		Trait("Category", "UnitTest")]
	public void UseProfile_AddProfiles_AddNone_ReturnsWithoutProfiles()
	{
		// Act
		var resultWithSetConfiguration = _sut
			.WithDefaultConfiguration()
			.UseProfiles(Array.Empty<TestSerializerProfile>());

		// Assert
		resultWithSetConfiguration.Mappings.Should().HaveCount(0);
	}

	private sealed class TestSerializerFactory : BaseSerializerFactory<TestSerializer, TestConfiguration, TestSerializerProfile>
	{
		public TestConfiguration GetDefaultConfiguration() => DefaultConfiguration;

		protected override TestConfiguration DefaultConfiguration => new();
		protected override TestSerializer CreateSerializer(
			in TestConfiguration configuration,
			in ObjectPoolProvider poolProvider,
			in IReadOnlyCollection<IClassMap> mappings) => new (configuration, poolProvider, mappings);
	}

	private sealed class TestSerializer : ISerializer
	{
		public TestSerializer(in TestConfiguration configuration, in ObjectPoolProvider poolProvider, in IReadOnlyCollection<IClassMap> mappings)
		{
			Configuration = configuration;
			PoolProvider = poolProvider;
			Mappings = mappings;
		}

		public ObjectPoolProvider PoolProvider { get; }

		public IReadOnlyCollection<IClassMap> Mappings { get; }

		public SerializerConfiguration Configuration { get; }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
		public string Serialize<TModel>(in TModel? model) where TModel : new() => throw new NotSupportedException("Out of test scope");
		public TModel Deserialize<TModel>(in string? stringData) where TModel : new() => throw new NotSupportedException("Out of test scope");
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
	}

	private sealed class TestConfiguration: SerializerConfiguration { }

	private sealed class TestSerializerProfile : ISerializerProfile<TestConfiguration>
	{
		public IReadOnlyCollection<IClassMap> Configure(in TestConfiguration configuration) => new List<IClassMap>
		{
			Mock.Of<IClassMap>(MockBehavior.Loose)
		};
	}
}
