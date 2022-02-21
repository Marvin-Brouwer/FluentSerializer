using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.SerializerException;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiles;
using FluentSerializer.Json.Services;
using FluentSerializer.Json.Tests.ObjectMother;
using Moq;
using System.Collections.Generic;
using FluentSerializer.Core.Context;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Services;

public sealed class JsonTypeSerializerTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;

	private readonly ISerializerCoreContext<IJsonNode> _coreContextStub;
	private readonly Mock<IClassMapScanList<JsonSerializerProfile>> _scanList;
	private readonly Mock<IClassMap> _classMap;

	public JsonTypeSerializerTests()
	{
		_coreContextStub = new SerializerCoreContext<IJsonNode>(Mock.Of<IAdvancedJsonSerializer>());
		_scanList = new Mock<IClassMapScanList<JsonSerializerProfile>>();
		_classMap = new Mock<IClassMap>()
			.WithNamingStrategy(Names.Use.PascalCase)
			.WithoutPropertyMaps();
	}

	/// <summary>
	/// We need a profile to serialize
	/// </summary>
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_TypeIsNotRegistered_Throws()
	{
		// Arrange
		var input = new TestClass();

		var type = typeof(TestClass);
		var scanList = Mock.Of<IClassMapScanList<JsonSerializerProfile>>();

		var sut = new JsonTypeSerializer(in scanList);

		// Act
		var result = () => sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ClassMapNotFoundException>()
			.Which.TargetType.Should().Be(type);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_NoPropertiesMapped_ReturnsEmptyNode()
	{
		// Arrange
		var expected = Object();
		var input = new TestClass
		{
			Value = "Never used"
		};

		var type = typeof(TestClass);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeSerializer(_scanList.Object);

		// Act
		var result = sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_InvalidPropertyMapping_Throws()
	{
		// Arrange
		var input = new TestClass
		{
			Value = "Never used"
		};
		var type = typeof(TestClass);

		// Any arbitrary type here
		var attemptedContainerType = typeof(bool);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, attemptedContainerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeSerializer(_scanList.Object);

		// Act
		var result = () => sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should()
			.ThrowExactly<ContainerNotSupportedException>()
			.Which.ContainerType.Should().Be(attemptedContainerType);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_ObjectPropertyMapping_ReturnsValue()
	{
		// Arrange
		var expected = Object(
			Property(nameof(TestClass.Value), Value("\"test\""))
		);
		var input = new TestClass
		{
			Value = "test"
		};

		var type = typeof(TestClass);
		var containerType = typeof(IJsonProperty);
		var targetProperty = type.GetProperty(nameof(TestClass.Value))!;
		_classMap
			.WithBasicProppertyMapping(TestDirection, containerType, targetProperty);
		_scanList
			.WithClassMap(type, _classMap);

		var sut = new JsonTypeSerializer(_scanList.Object);

		// Act
		var result = sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void SerializeToNode_TypeIsEnumerable_ReturnsArray()
	{
		// Arrange
		var expected = Array();
		var input = new List<int>();

		var type = typeof(IEnumerable<int>);
		var scanList = Mock.Of<IClassMapScanList<JsonSerializerProfile>>();

		var sut = new JsonTypeSerializer(in scanList);

		// Act
		var result = sut.SerializeToNode(input, type, _coreContextStub);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	private sealed class TestClass
	{
		public string Value { get; init; } = default!;
	}
}
