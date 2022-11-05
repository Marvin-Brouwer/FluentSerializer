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

public sealed class ClassMapCollectionTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;
	private static readonly Func<INamingStrategy> TestNames = Names.Equal("Test");
	private static readonly PropertyInfo CorrectProperty = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;

	private static readonly IReadOnlyCollection<IPropertyMap> PropertyMaps = new List<IPropertyMap>
	{
		new PropertyMap(TestDirection, typeof(bool), CorrectProperty, TestNames, null)
	};
	private static readonly IReadOnlyCollection<IClassMap> ClassMaps = new List<IClassMap>
	{
		new ClassMap(typeof(TestClass), TestDirection, TestNames, PropertyMaps)
	};

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetClassMapFor_TypeNull_Throws()
	{
		// Arrange
		var type = (Type)null!;
		var direction = SerializerDirection.Serialize;
		var sut = new ClassMapCollection(in ClassMaps);

		// Act
		var result1 = () => sut.GetClassMapFor(in type, in direction);
		var result2 = () => sut.GetClassMapFor(in type);

		// Assert
		result1.Should().ThrowExactly<ArgumentNullException>();
		result2.Should().ThrowExactly<ArgumentNullException>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetClassMapFor_SerializerDirectionBoth_Throws()
	{
		// Arrange
		var type = typeof(TestClass);
		var direction = SerializerDirection.Both;
		var sut = new ClassMapCollection(in ClassMaps);

		// Act
		var result = () => sut.GetClassMapFor(in type, in direction);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithMessage(
				"You cannot get a ClassMap for SerializerDirection.Both *"+
			    "you can only register one as such! (Parameter 'direction')")
			.WithParameterName(nameof(direction));
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetClassMapFor_IncorrectType_NoMatch_ReturnsNone()
	{
		// Arrange
		var type = typeof(int);
		var direction = SerializerDirection.Serialize;
		var sut = new ClassMapCollection(in ClassMaps);

		// Act
		var result = sut.GetClassMapFor(in type, in direction);
		var result2 = sut.GetClassMapFor(in type);

		// Assert
		result.Should().BeNull();
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetClassMapFor_IncorrectDirection_NoMatch_ReturnsNone()
	{
		// Arrange
		var type = typeof(TestClass);
		var direction = SerializerDirection.Deserialize;
		var sut = new ClassMapCollection(in ClassMaps);

		// Act
		var result = sut.GetClassMapFor(in type, in direction);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetClassMapFor_Match_ReturnsClassMap()
	{
		// Arrange
		var type = typeof(TestClass);
		var direction = SerializerDirection.Serialize;
		var sut = new ClassMapCollection(in ClassMaps);

		// Act
		var result = sut.GetClassMapFor(in type, in direction);
		var result2 = sut.GetClassMapFor(in type);

		// Assert
		result.Should().NotBeNull();
		result2.Should().NotBeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void GetClassMapFor_ClassMapForBoth_ReturnsClassMap()
	{
		// Arrange
		var type = typeof(TestClass);
		var direction = SerializerDirection.Serialize;
		var sut = new ClassMapCollection(new List<IClassMap>
		{
			new ClassMap(typeof(TestClass), SerializerDirection.Both, TestNames, PropertyMaps)
		});

		// Act
		var result = sut.GetClassMapFor(in type, in direction);
		var result2 = sut.GetClassMapFor(in type);

		// Assert
		result.Should().NotBeNull();
		result2.Should().NotBeNull();
	}

	private sealed  class TestClass {
		public int Id { get; init; } = default!;
	}
}