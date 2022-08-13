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

	private static readonly IReadOnlyCollection<IPropertyMap> PropertyMaps = new List<IPropertyMap>()
	{
		new PropertyMap(TestDirection, typeof(bool), CorrectProperty, TestNames, null)
	};
	private static readonly IReadOnlyCollection<IClassMap> ClassMaps = new List<IClassMap>()
	{
		new ClassMap(typeof(TestClass), TestDirection, TestNames, PropertyMaps)
	};

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
		result.Should().ThrowExactly<NotSupportedException>();
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

	private sealed  class TestClass {
		public int Id { get; set; }
	}
}