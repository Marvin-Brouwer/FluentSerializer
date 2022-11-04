using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Tests.ObjectMother;

using Moq;

using System;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Context;

public sealed class NamingContextTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Both;

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
	public void FindNamingStrategy_ForType_TypeNull_Throws()
	{
		// Arrange
		var type = (Type)null!;

		var sut = new NamingContext(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.FindNamingStrategy(in type);

		// Assert
		result.Should().ThrowExactly<ArgumentNullException>();
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

	[Theory,
		InlineData(null, null),
		InlineData(typeof(TestClass), null),
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForProperty_ValueNull_Throws(Type type, PropertyInfo property)
	{
		// Arrange
		var sut = new NamingContext(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.FindNamingStrategy(in type, in property);

		// Assert
		result.Should().ThrowExactly<ArgumentNullException>();
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
			.WithBasicPropertyMapping(TestDirection, typeof(ISerializerProfile<ISerializerConfiguration>), property, null!);
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

	[Fact,
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_ForPropertyMapCollection_ValueNull_Throws()
	{
		// Arrange
		var propertyMapping = (IPropertyMapCollection)null!;
		var propertyMappingMock = new Mock<IPropertyMapCollection>(MockBehavior.Strict);
		var property = (PropertyInfo)null!;


		// Act
		var result1 = () => NamingContext.FindNamingStrategy(in propertyMapping, in property);
		var result2 = () => NamingContext.FindNamingStrategy(propertyMappingMock.Object, in property);

		// Assert
		result1.Should().ThrowExactly<ArgumentNullException>();
		result2.Should().ThrowExactly<ArgumentNullException>();
	}

	private sealed class TestClass
	{
		public int Id { get; init; } = default!;
	}
}