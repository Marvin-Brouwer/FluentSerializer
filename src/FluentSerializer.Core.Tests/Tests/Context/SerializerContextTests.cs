using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Core.Services;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.Text;

using Moq;

using System;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Context;

/// <remarks>
/// These tests mainly focus on the typed sut since the methods on <see cref="SerializerCoreContext{T}"/> proxy through
/// to the counterparts in <see cref="SerializerCoreContext"/>.
/// However for null checking the regular sut is used, since technically they can be accessed with null values.
/// </remarks>
public sealed class SerializerContextTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Both;

	private readonly Mock<IClassMapCollection> _classMapCollectionMock;
	private readonly Mock<IClassMap> _classMapMock;
	private readonly Mock<ISerializer> _serializerMock;
	private readonly ISerializerCoreContext _coreContext;
	private readonly ISerializerCoreContext<TestClass> _typedCoreContext;

	public SerializerContextTests()
	{
		_classMapCollectionMock = new Mock<IClassMapCollection>();
		_classMapMock = new Mock<IClassMap>()
			.WithoutPropertyMaps();
		_serializerMock = new Mock<ISerializer>()
			.UseConfig(TestSerializerConfiguration.Default);

		_coreContext = new SerializerCoreContext(_serializerMock.Object);
		_typedCoreContext = new SerializerCoreContext<TestClass>(_serializerMock.Object);
	}

	#region INamingContext

	[Theory,
		InlineData(typeof(int)), InlineData(typeof(int?)),
		Trait("Category", "UnitTest")]
	public void FindNamingStrategy_Constructor(Type input)
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;

		// Act
		var result = new SerializerContext<TestClass>(_typedCoreContext, property, input, type,
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

		_classMapMock
			.WithBasicPropertyMapping(TestDirection, typeof(ISerializerProfile<ISerializerConfiguration>), property, null!);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new SerializerContext<TestClass>(_typedCoreContext, property, property.PropertyType, type,
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

		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new SerializerContext<TestClass>(_typedCoreContext, property, property.PropertyType, type,
			Names.Use.KebabCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = sut.FindNamingStrategy(in property);

		// Assert
		result.Should().BeNull();
	}

	#endregion

	#region ISerializerCoreContext
	
	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_Class_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);

		var sut = new SerializerContext<TestClass>(_typedCoreContext,
			Mock.Of<PropertyInfo>(), Mock.Of<Type>(), Mock.Of<Type>(),
			Names.Use.CamelCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = sut
			.WithPathSegment(type);

		// Assert
		result.Path.Should().BeEquivalentTo("T:TestClass");
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_Property_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;

		var sut = new SerializerContext<TestClass>(_typedCoreContext,
			Mock.Of<PropertyInfo>(), Mock.Of<Type>(), Mock.Of<Type>(),
			Names.Use.CamelCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = sut
			.WithPathSegment(property);

		// Assert
		result.Path.Should().BeEquivalentTo("P:Id");
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void WithPathSegment_ClassAndProperty_ReturnsExpectedPath()
	{
		// Arrange
		var type = typeof(TestClass);
		var property = type.GetProperty(nameof(TestClass.Id))!;

		var sut = new SerializerContext<TestClass>(_typedCoreContext,
			Mock.Of<PropertyInfo>(), Mock.Of<Type>(), Mock.Of<Type>(),
			Names.Use.CamelCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = sut
			.WithPathSegment(type)
			.WithPathSegment(property);

		// Assert
		result.Path.Should().BeEquivalentTo("T:TestClass", "P:Id");
	}


	[Theory,
		InlineData(null, null),
		InlineData(typeof(TestClass), null),
		Trait("Category", "UnitTest")]
	public void WithPathSegment_ValueNull_Throws(Type type, PropertyInfo property)
	{
		// Arrange
		var sut = new SerializerContext(_coreContext,
			Mock.Of<PropertyInfo>(), Mock.Of<Type>(), Mock.Of<Type>(),
			Names.Use.CamelCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);
		var typedSut = new SerializerContext<TestClass>(_typedCoreContext,
			Mock.Of<PropertyInfo>(), Mock.Of<Type>(), Mock.Of<Type>(),
			Names.Use.CamelCase(), _classMapMock.Object.PropertyMapCollection, _classMapCollectionMock.Object);

		// Act
		var result = () => sut
			.WithPathSegment(type)
			.WithPathSegment(property);
		var typedResult = () => typedSut
			.WithPathSegment(type)
			.WithPathSegment(property);
		// Assert
		result.Should().ThrowExactly<ArgumentNullException>();
		typedResult.Should().ThrowExactly<ArgumentNullException>();
	}

	#endregion

	private sealed class TestClass : IDataNode
	{
		public int Id { get; init; } = default!;

		public bool Equals(IDataNode? other) => false;
		public HashCode GetNodeHash() => DataNodeHashingHelper.GetHashCodeForAll(Id);

		public string Name => throw new NotSupportedException("Out of test scope");
		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) =>
			throw new NotSupportedException("Out of test scope");
	}

	private sealed class TestSerializerConfiguration : SerializerConfiguration
	{
		public static readonly TestSerializerConfiguration Default = new ();
	}
}