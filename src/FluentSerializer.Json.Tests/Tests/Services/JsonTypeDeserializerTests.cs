using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using FluentSerializer.Json.Tests.ObjectMother;

using Moq;

using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Services;

public sealed class JsonTypeDeserializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Deserialize;

	private readonly ISerializerCoreContext<IJsonNode> _coreContextStub;
	private readonly Mock<IClassMap> _classMapMock;
	private readonly Mock<IClassMapCollection> _classMapCollectionMock;

	public JsonTypeDeserializerTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_coreContextStub = new SerializerCoreContext<IJsonNode>(serializerMock.Object);
		_classMapMock = new Mock<IClassMap>()
			.WithDefaults()
			.WithoutPropertyMaps();
		_classMapCollectionMock = new Mock<IClassMapCollection>();
	}

	/// <summary>
	/// We need a profile to deserialize
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = Object();

		var type = typeof(TestClass);

		_classMapCollectionMock
			.WithoutClassMaps();

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ClassMapNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	/// <summary>
	/// We need a node to get values
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_NodeIsNotPresent_Throws()
	{
		// Arrange
		var input = Object(Property("IncorrectName", Value(null)));

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var property = type.GetProperty(nameof(TestClass.Value));

		_classMapMock
			.WithClassType(type)
			.WithBasicPropertyMapping(TestDirection, containerType, property!);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_NonNullableValueIsNull_Throws()
	{
		// Arrange
		var input = Object(Property(nameof(TestClass.Value), Value(null)));

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var property = type.GetProperty(nameof(TestClass.Value));

		_classMapMock
			.WithClassType(type)
			.WithBasicPropertyMapping(TestDirection, containerType, property!);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var input = Object();

		var type = typeof(TestClass);

		_classMapMock
			.WithClassType(type);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromNode(input, type, _coreContextStub);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType(type);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_InvalidPropertyMapping_Throws()
	{
		// Arrange
		var input = Object();
		var type = typeof(TestClass);

		// Any arbitrary type here
		var attemptedContainerType = typeof(bool);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicPropertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_ObjectPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = new TestClass
		{
			Value = "test"
		};
		var input = Object(
			Property(nameof(TestClass.Value), Value("\"test\""))
		);

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;

		_classMapMock
			.WithClassType(type)
			.WithBasicPropertyMapping(TestDirection, containerType, targetProperty);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_TypeIsEnumerable_ReturnsEnumerable()
	{
		// Arrange
		var expected = new List<IJsonObject>
		{
			Object(),
			Object()
		};
		var input = Array(
			Object(),
			Object()
		);

		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.WithDeserialize();
		var contextMock = new Mock<ISerializerCoreContext<IJsonNode>>()
			.WithAutoPathSegment()
			.WithSerializer(serializerMock);
		_classMapCollectionMock
			.WithClassMap(_classMapMock);

		var sut = new JsonTypeDeserializer(_classMapCollectionMock.Object);

		// Act
		var result = sut.DeserializeFromNode(input, typeof(IEnumerable<IJsonObject>), contextMock.Object);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private sealed class TestClass
	{
		public string Value { get; init; } = default!;
	}
}
