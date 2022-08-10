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
	[Obsolete("Obsolete", false)]
	public void ScanAssembliesForType_NotPresent_ReturnsEmptyList()
	{
		// Arrange
		_assemblyMock
			.Setup(assembly => assembly.GetTypes())
			.Returns(Array.Empty<Type>());

		// Act
		var result = ProfileScanner
			.FindClassMapsInAssembly<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(_assemblyMock.Object, _configurationMock.Object);

		// Assert
		result.Should().BeEmpty();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	[Obsolete("Obsolete", false)]
	public void ScanAssembliesForType_Present_ReturnsProfile()
	{
		// Arrange
		_assemblyMock
			.Setup(assembly => assembly.GetTypes())
			.Returns(new[] { typeof(SerializerProfileFake) });

		// Act
		var result = ProfileScanner
			.FindClassMapsInAssembly<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(_assemblyMock.Object, _configurationMock.Object);

		// Assert
		result.Should().NotBeEmpty();
	}

	private sealed class SerializerProfileFake : ISerializerProfile<ISerializerConfiguration>
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