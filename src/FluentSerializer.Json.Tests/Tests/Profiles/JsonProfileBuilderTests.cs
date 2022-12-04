using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiles;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace FluentSerializer.Json.Tests.Tests.Profiles;

public sealed class JsonProfileBuilderTests
{
	private static JsonProfileBuilder<TestModel> CreateSut(in List<IPropertyMap> propertyMap)
	{
		var defaultNamingStrategy = Names.Use.CamelCase;

		return new(in defaultNamingStrategy, in propertyMap);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Initialize_NullValues_Throws()
	{
		// Arrange
		var defaultNamingStrategy = Names.Use.CamelCase;
		var propertyMap = new List<IPropertyMap>();

		// Act
		var result1 = () => new JsonProfileBuilder<TestModel>(null!, in propertyMap);
		var result2 = () => new JsonProfileBuilder<TestModel>(in defaultNamingStrategy, null!);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(defaultNamingStrategy));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(propertyMap));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Property_WithDifferentConfigurations_ReturnsExpected()
	{
		// Arrange
		var expected1 = new PropertyMap(
			SerializerDirection.Both,
			typeof(IJsonProperty),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			Names.Use.CamelCase,
			null
		);
		var expected2 = new PropertyMap(
			SerializerDirection.Serialize,
			typeof(IJsonProperty),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			Names.Use.KebabCase,
			TestConverter.New
		);

		var propertyMaps = new List<IPropertyMap>();
		var sut = CreateSut(in propertyMaps);

		// Act
		sut.Property(model => model.Name);
		sut.Property(model => model.Name,
			SerializerDirection.Serialize,
			Names.Use.KebabCase,
			TestConverter.New
		);

		// Assert
		propertyMaps.First().Should().BeEquivalentTo(expected1);
		propertyMaps.Last().Should().BeEquivalentTo(expected2);
	}

	private sealed class TestModel
	{
		public string Name => nameof(TestModel);
	}

	private sealed class TestConverter: IJsonConverter
	{
		public static IJsonConverter New() => new TestConverter();

		public bool CanConvert(in Type targetType) => false;
		public SerializerDirection Direction => SerializerDirection.Both;
		public Guid ConverterId { get; } = typeof(TestConverter).GUID;

		public IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context) =>
			throw new NotSupportedException("Out of test scope");
		public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext<IJsonNode> context) =>
			throw new NotSupportedException("Out of test scope");
	}
}
