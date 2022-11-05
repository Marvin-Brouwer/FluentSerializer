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
		var classType = typeof(int);
		var namingStrategy = Names.Use.CamelCase;
		var propertyMap = Array.Empty<PropertyMap>();

		// Act
		var result1 = () => new ClassMap(null!, SerializerDirection.Serialize, namingStrategy, propertyMap);
		var result2 = () => new ClassMap(classType, SerializerDirection.Serialize, null!, propertyMap);
		var result3 = () => new ClassMap(classType, SerializerDirection.Serialize, namingStrategy, null!);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(classType));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(namingStrategy));
		result3.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(propertyMap));
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
