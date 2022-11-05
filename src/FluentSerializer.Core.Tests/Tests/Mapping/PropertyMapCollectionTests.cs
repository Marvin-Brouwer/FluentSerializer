using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Mapping;

public sealed class PropertyMapCollectionTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;
	private static readonly Func<INamingStrategy> TestNames = Names.Equal("Test");
	private static readonly PropertyInfo CorrectProperty = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;

	private static readonly IReadOnlyCollection<IPropertyMap> PropertyMaps = new List<IPropertyMap>
	{
		new PropertyMap(TestDirection, typeof(bool), CorrectProperty, TestNames, null)
	};

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetAllPropertyMaps_DirectionBoth_Throws()
	{
		// Arrange
		var direction = SerializerDirection.Both;
		var sut = new PropertyMapCollection(in PropertyMaps);

		// Act
		var result = () => sut.GetAllPropertyMaps(in direction);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithMessage(
				"You cannot get a PropertyMap for SerializerDirection.Both *" +
				"you can only register one as such! (Parameter 'direction')")
			.WithParameterName(nameof(direction));
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetAllPropertyMaps_IncorrectDirection_ReturnsNone()
	{
		// Arrange
		var direction = SerializerDirection.Deserialize;
		var sut = new PropertyMapCollection(in PropertyMaps);

		// Act
		var result = sut.GetAllPropertyMaps(in direction);

		// Assert
		result.Should().BeEmpty();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetAllPropertyMaps_CorrectDirection_ReturnsPropertyMap()
	{
		// Arrange
		var direction = SerializerDirection.Serialize;
		var sut = new PropertyMapCollection(in PropertyMaps);

		// Act
		var result = sut.GetAllPropertyMaps(in direction);

		// Assert
		result.Should().NotBeEmpty();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetPropertyMapFor_IncorrectProperty_NoMatch_ReturnsNone()
	{
		// Arrange
		var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Name))!;
		var direction = SerializerDirection.Serialize;
		var sut = new PropertyMapCollection(in PropertyMaps);

		// Act
		var result = sut.GetPropertyMapFor(in propertyInfo, in direction);
		var result2 = sut.GetPropertyMapFor(in propertyInfo);

		// Assert
		result.Should().BeNull();
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetPropertyMapFor_IncorrectDirection_NoMatch_ReturnsNone()
	{
		// Arrange
		var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;
		var direction = SerializerDirection.Deserialize;
		var sut = new PropertyMapCollection(in PropertyMaps);

		// Act
		var result = sut.GetPropertyMapFor(in propertyInfo, in direction);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetPropertyMapFor_Match_ReturnsPropertyMap()
	{
		// Arrange
		var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;
		var direction = SerializerDirection.Serialize;
		var sut = new PropertyMapCollection(in PropertyMaps);

		// Act
		var result = sut.GetPropertyMapFor(in propertyInfo, in direction);
		var result2 = sut.GetPropertyMapFor(in propertyInfo);

		// Assert
		result.Should().NotBeNull();
		result2.Should().NotBeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetPropertyMapFor_Match_PropertyMapForBoth_ReturnsPropertyMap()
	{
		// Arrange
		var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;
		var direction = SerializerDirection.Serialize;
		var sut = new PropertyMapCollection(new List<IPropertyMap>
		{
			new PropertyMap(SerializerDirection.Both, typeof(bool), CorrectProperty, TestNames, null)
		});

		// Act
		var result = sut.GetPropertyMapFor(in propertyInfo, in direction);
		var result2 = sut.GetPropertyMapFor(in propertyInfo);

		// Assert
		result.Should().NotBeNull();
		result2.Should().NotBeNull();
	}

	private sealed  class TestClass {
		public int Id { get; init; } = default!;
		public string Name { get; init; } = default!;
	}
}