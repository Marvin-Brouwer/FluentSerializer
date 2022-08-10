using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using FluentSerializer.Json.Tests.ObjectMother;

using Moq;

using System;
using System.Collections.Generic;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Test the converting between <see cref="List{T}"/> and a collection of JSON elements.
/// <code>
/// <![CDATA[
/// {
///		"list": [
///			{ },
///			{ },
///			{ }
///		]
///	}
/// ]]>
/// </code>
/// These tests compare a json object with children to a list to validate the converter is doing it's job.
/// </summary>
public sealed class CollectionConverterTests
{
	private const string ListName = "list";

	private readonly CollectionConverter _sut;
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;
	private readonly Mock<IAdvancedJsonSerializer> _serializerMock;

	public CollectionConverterTests()
	{
		_sut = new CollectionConverter();
		_serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(_serializerMock)
			.WithNamingStrategy(Names.Equal(ListName));
	}

	#region Failing checks
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Serialize_NotEnumerable_Throws()
	{
		// Arrange
		// Give it any arbitrary object that doesn't implement IEnumerable
		var input = false;

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = () => _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<NotSupportedException>()
			.WithMessage("Type 'System.Boolean' does not implement IEnumerable");
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_NotEnumerable_Throws()
	{
		// Arrange
		var input = Array();

		_contextMock
			// Give it any arbitrary type that doesn't implement IEnumerable
			.WithPropertyType<bool>();

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = () => _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<NotSupportedException>()
			.WithMessage("Unable to create an enumerable collection of 'System.Boolean'");
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_NotArray_Throws()
	{
		// Arrange
		var input = Object();

		_contextMock
			// Give it any arbitrary type that doesn't implement IEnumerable
			.WithPropertyType<IEnumerable<int>>();

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = () => _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<NotSupportedException>()
			.WithMessage("The json object you attempted to deserialize was not a collection");
	}
	#endregion

	#region Serialize
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Serialize_EmptyListOfIJsonObject()
	{
		// Arrange
		var expected = Array();
		var input = new List<IJsonObject>(0);

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Serialize_ListOfIJsonObject()
	{
		// Arrange
		var expected = Array(
			Object(),
			Object()
		);
		var input = new List<IJsonObject> {
			Object(),
			Object()
		};

		_serializerMock
			.WithSerializeToContainer();

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	#region Deserialize
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_EmptyListOfIJsonObject()
	{
		// Arrange
		var expected = new List<IJsonObject>(0);
		var input = Array();

		_contextMock
			.WithFindNamingStrategy(Names.Equal(ListName))
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = _sut.CanConvert(expected.GetType());
		var result = _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_ListOfIJsonObject()
	{
		// Arrange
		var expected = new List<IJsonObject> {
			Object(),
			Object()
		};
		var input = Array(
			Object(),
			Object()
		);

		_serializerMock
			.WithDeserialize();
		_contextMock
			.WithFindNamingStrategy(Names.Equal(ListName))
			.WithPropertyType(expected.GetType());

		// Act
		var canConvert = _sut.CanConvert(expected.GetType());
		var result = _sut.Deserialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion
}

