using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Tests.ObjectMother;

using Moq;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Context;

public sealed class NamingContextTests
{
	private static readonly SerializerDirection TestDirection = SerializerDirection.Both;

	private readonly Mock<IClassMapCollection> _classMapCollectionMock;
	private readonly Mock<IClassMap> _classMapMock;

	public NamingContextTests()
	{
		_classMapCollectionMock = new Mock<IClassMapCollection>();
		_classMapMock = new Mock<IClassMap>()
			.WithoutPropertyMaps();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForType_ReturnsStrategy()
	{
		// Arrange
		var type = typeof(TestClass);

		_classMapMock
			.WithClassType(type)
			.WithNamingStrategy(Names.Use.CamelCase);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new NamingContext(_classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in type);

		// Assert
		result.Should().BeEquivalentTo(Names.Use.CamelCase());
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForType_NotFound_ReturnsNull()
	{
		// Arrange
		var type = typeof(TestClass);

		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new NamingContext(_classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in type);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForProperty_ReturnsStrategy()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicProppertyMapping(TestDirection, typeof(ISerializerProfile<ISerializerConfiguration>), property, null!);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new NamingContext(_classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in type, in property);

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

		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new NamingContext(_classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in type, in property);

		// Assert
		result.Should().BeNull();
	}

	private sealed class TestClass
	{
		public int Id { get; init; } = default!;
	}
}