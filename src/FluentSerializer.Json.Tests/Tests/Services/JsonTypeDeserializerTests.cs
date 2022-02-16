using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.TestUtils.ObjectMother;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiles;
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

	private readonly Mock<IAdvancedJsonSerializer> _serializerMock;
	private readonly Mock<IClassMapScanList<JsonSerializerProfile>> _scanList;
	private readonly Mock<IClassMap> _classMap;

	public JsonTypeDeserializerTests()
	{
		_serializerMock = new Mock<IAdvancedJsonSerializer>();
		_scanList = new Mock<IClassMapScanList<JsonSerializerProfile>>();
		_classMap = new Mock<IClassMap>()
			.WithNamingStrategy(Names.Use.PascalCase)
			.WithoutPropertyMaps();
	}

	/// <summary>
	/// We need a profile to deserialize
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = Object();

		var type = typeof(TestClass);
		var scanList = Mock.Of<IClassMapScanList<JsonSerializerProfile>>();

		var sut = new JsonTypeDeserializer(in scanList);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<ClassMapNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	/// <summary>
	/// We need a node to get values
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_NodeIsNotPresent_Throws()
	{
		// Arrange
		var input = Object(Property("IncorrectName", Value(null)));

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var property = type.GetProperty(nameof(TestClass.Value));
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, property!);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeDeserializer(_scanList.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotFoundException>()
			.Which.ContainerType.Should().Be(containerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_NonNullableValueIsNull_Throws()
	{
		// Arrange
		var input = Object(Property(nameof(TestClass.Value), Value(null)));

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var property = type.GetProperty(nameof(TestClass.Value));
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, property!);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeDeserializer(_scanList.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotFoundException>()
			.Which.ContainerType.Should().Be(containerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var input = Object();

		var type = typeof(TestClass);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeDeserializer(_scanList.Object);

		// Act
		var result = sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void DeserializeFromNode_InvalidPropertyMapping_Throws()
	{
		// Arrange
		var input = Object();
		var type = typeof(TestClass);

		// Any arbitrary type here
		var attemptedContainerType = typeof(bool);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeDeserializer(_scanList.Object);

		// Act
		var result = () => sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
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
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeDeserializer(_scanList.Object);

		// Act
		var result = sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
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

		var type = typeof(IEnumerable<IJsonObject>);
		_serializerMock
			.WithDeserialize();

		var sut = new JsonTypeDeserializer(_scanList.Object);

		// Act
		var result = sut.DeserializeFromNode(input, type, _serializerMock.Object);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private sealed class TestClass
	{
		public string Value { get; set; } = default!;
	}
}