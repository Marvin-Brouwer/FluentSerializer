using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Services;
using FluentSerializer.Core.Tests.ObjectMother;

using Moq;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Context;

public sealed class SerializerContextTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Both;

	private readonly Mock<IClassMapCollection> _classMapCollectionMock;
	private readonly Mock<IClassMap> _classMapMock;
	private readonly Mock<ISerializer> _serializerMock;

	public SerializerContextTests()
	{
		_classMapCollectionMock = new Mock<IClassMapCollection>();
		_classMapMock = new Mock<IClassMap>()
			.WithoutPropertyMaps();
		_serializerMock = new Mock<ISerializer>()
			.UseConfig(TestSerializerConfiguration.Default);
	}

	[Theory,
		InlineData(typeof(int)), InlineData(typeof(int?)),
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_Constructor(Type input)
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;
		var coreContext = new SerializerCoreContext(_serializerMock.Object);

		// Act
		var result = new SerializerContext(coreContext, property, input, type,
			Names.Use.KebabCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Assert
		result.ClassType.Should().Be(typeof(TestClass));
		result.PropertyType.Should().Be(typeof(int));
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForProperty_ReturnsStrategy()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;
		var coreContext = new SerializerCoreContext(_serializerMock.Object);

		_classMapMock
			.WithBasicPropertyMapping(TestDirection, typeof(ISerializerProfile<ISerializerConfiguration>), property, null!);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new SerializerContext(coreContext, property, property.PropertyType, type,
			Names.Use.KebabCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in property);

		// Assert
		result.Should().BeEquivalentTo(Names.Use.PascalCase());
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForProperty_NotFound_ReturnsNull()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;
		var coreContext = new SerializerCoreContext(_serializerMock.Object);

		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new SerializerContext(coreContext, property, property.PropertyType, type,
			Names.Use.KebabCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in property);

		// Assert
		result.Should().BeNull();
	}

	private sealed class TestClass
	{
		public int Id { get; init; } = default!;
	}

	private sealed class TestSerializerConfiguration : SerializerConfiguration
	{
		public static readonly TestSerializerConfiguration Default = new ();
	}
}