using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Profiles;

using Moq;

using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Profiles;

public sealed class ProfileScannerTests
{
	private readonly Mock<SerializerConfiguration> _configurationMock = new();
	private readonly Mock<Assembly> _assemblyMock;

	public ProfileScannerTests()
	{
		_assemblyMock = new();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ScanAssembliesForType_NotPresent_ReturnsEmptyList()
	{
		// Arrange
		_assemblyMock
			.Setup(assembly => assembly.GetTypes())
			.Returns(Array.Empty<Type>());

		// Act
		var profiles = ProfileScanner
			.ScanAssembly<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(_assemblyMock.Object);
		var result = ProfileScanner.
			FindClassMapsInProfiles<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(profiles, _configurationMock.Object);

		// Assert
		result.Should().BeEmpty();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ScanAssembliesForType_Abstract_ReturnsEmptyList()
	{
		// Arrange
		_assemblyMock
			.Setup(assembly => assembly.GetTypes())
			.Returns(new[] { typeof(BaseSerializerProfileFake) });

		// Act
		var profiles = ProfileScanner
			.ScanAssembly<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(_assemblyMock.Object);
		var result = ProfileScanner.
			FindClassMapsInProfiles<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(profiles, _configurationMock.Object);

		// Assert
		result.Should().BeEmpty();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ScanAssembliesForType_NonProfile_ReturnsEmptyList()
	{
		// Arrange
		_assemblyMock
			.Setup(assembly => assembly.GetTypes())
			.Returns(new[] { typeof(ProfileScannerTests) });

		// Act
		var profiles = ProfileScanner
			.ScanAssembly<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(_assemblyMock.Object);
		var result = ProfileScanner.
			FindClassMapsInProfiles<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(profiles, _configurationMock.Object);

		// Assert
		result.Should().BeEmpty();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ScanAssembliesForType_Present_ReturnsProfile()
	{
		// Arrange
		_assemblyMock
			.Setup(assembly => assembly.GetTypes())
			.Returns(new[] { typeof(SerializerProfileFake) });

		// Act
		var profiles = ProfileScanner
			.ScanAssembly<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(_assemblyMock.Object);
		var result = ProfileScanner.
			FindClassMapsInProfiles<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(profiles, _configurationMock.Object);

		// Assert
		result.Should().NotBeEmpty();
	}

	private sealed class SerializerProfileFake : BaseSerializerProfileFake { }

	private abstract class BaseSerializerProfileFake : ISerializerProfile<ISerializerConfiguration>
	{
		private static readonly List<IClassMap> ClassMaps = new()
		{
			Mock.Of<IClassMap>()
		};

		public IReadOnlyCollection<IClassMap> Configure(in ISerializerConfiguration configuration)
		{
			return ClassMaps;
		}
	}
}