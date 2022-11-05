using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;

using System;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Mapping;

public sealed class ClassMapTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void NewPropertyMap_NullValues_Throws()
	{
		// Arrange
		var type = typeof(int);
		var namingStrategy = Names.Use.CamelCase;

		// Act
		var result1 = () => new ClassMap(null!, SerializerDirection.Serialize, null!, null!);
		var result2 = () => new ClassMap(type, SerializerDirection.Serialize, null!, null!);
		var result3 = () => new ClassMap(type, SerializerDirection.Serialize, namingStrategy, null!);

		// Assert
		result1.Should().ThrowExactly<ArgumentNullException>();
		result2.Should().ThrowExactly<ArgumentNullException>();
		result3.Should().ThrowExactly<ArgumentNullException>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void NewClassMap_AnyType_HasConcreteType()
	{
		// Arrange
		var nullableType = typeof(int?);
		var concreteType = typeof(int);

		// Act
		var nullableResult = new ClassMap(nullableType, SerializerDirection.Serialize, Names.Use.CamelCase, Array.Empty<PropertyMap>());
		var concreteResult = new ClassMap(concreteType, SerializerDirection.Serialize, Names.Use.CamelCase, Array.Empty<PropertyMap>());

		// Assert
		nullableResult.ClassType.Should().Be(concreteType);
		nullableResult.ClassType.Should().NotBe(nullableType);
		concreteResult.ClassType.Should().Be(concreteType);
		concreteResult.ClassType.Should().NotBe(nullableType);
	}
}
