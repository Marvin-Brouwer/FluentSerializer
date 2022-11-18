using FluentAssertions;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Profiles;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests.Profiles;

public sealed class XmlProfileBuilderTests
{
	private static XmlProfileBuilder<TestModel> CreateSut(in List<IPropertyMap> propertyMap)
	{
		var defaultNamingStrategy = Names.Use.CamelCase;

		return new(in defaultNamingStrategy, in propertyMap);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Initialize_NullValues_Throws()
	{
		// Arrange
		var defaultNamingStrategy = Names.Use.CamelCase;
		var propertyMap = new List<IPropertyMap>();

		// Act
		var result1 = () => new XmlProfileBuilder<TestModel>(null!, in propertyMap);
		var result2 = () => new XmlProfileBuilder<TestModel>(in defaultNamingStrategy, null!);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(defaultNamingStrategy));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(propertyMap));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Attribute_WithDifferentConfigurations_ReturnsExpected()
	{
		// Arrange
		var expected1 = new PropertyMap(
			SerializerDirection.Both,
			typeof(IXmlAttribute),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			Names.Use.CamelCase,
			null
		);
		var expected2 = new PropertyMap(
			SerializerDirection.Serialize,
			typeof(IXmlAttribute),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			Names.Use.KebabCase,
			TestConverter.New
		);

		var propertyMaps = new List<IPropertyMap>();
		var sut = CreateSut(in propertyMaps);

		// Act
		sut.Attribute(model => model.Name);
		sut.Attribute(model => model.Name,
			SerializerDirection.Serialize,
			Names.Use.KebabCase,
			TestConverter.New
		);

		// Assert
		propertyMaps.First().Should().BeEquivalentTo(expected1);
		propertyMaps.Last().Should().BeEquivalentTo(expected2);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Child_WithDifferentConfigurations_ReturnsExpected()
	{
		// Arrange
		var expected1 = new PropertyMap(
			SerializerDirection.Both,
			typeof(IXmlElement),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			Names.Use.CamelCase,
			null
		);
		var expected2 = new PropertyMap(
			SerializerDirection.Serialize,
			typeof(IXmlElement),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			Names.Use.KebabCase,
			TestConverter.New
		);

		var propertyMaps = new List<IPropertyMap>();
		var sut = CreateSut(in propertyMaps);

		// Act
		sut.Child(model => model.Name);
		sut.Child(model => model.Name,
			SerializerDirection.Serialize,
			Names.Use.KebabCase,
			TestConverter.New
		);

		// Assert
		propertyMaps.First().Should().BeEquivalentTo(expected1);
		propertyMaps.Last().Should().BeEquivalentTo(expected2);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Text_WithDifferentConfigurations_ReturnsExpected()
	{
		// Arrange
		var expected1 = new PropertyMap(
			SerializerDirection.Both,
			typeof(IXmlText),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			TextNamingStrategy.Default,
			null
		);
		var expected2 = new PropertyMap(
			SerializerDirection.Serialize,
			typeof(IXmlText),
			typeof(TestModel).GetProperty(nameof(TestModel.Name))!,
			TextNamingStrategy.Default,
			TestConverter.New
		);

		var propertyMaps = new List<IPropertyMap>();
		var sut = CreateSut(in propertyMaps);

		// Act
		sut.Text(model => model.Name);
		sut.Text(model => model.Name,
			SerializerDirection.Serialize,
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

	private sealed class TestConverter :
		IXmlConverter<IXmlAttribute>,
		IXmlConverter<IXmlElement>,
		IXmlConverter<IXmlText>
	{
		public static TestConverter New() => new ();

		public bool CanConvert(in Type targetType) => false;
		public SerializerDirection Direction => SerializerDirection.Both;
		public int ConverterHashCode => 0000001;

		IXmlAttribute? IConverter<IXmlAttribute, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context) =>
			throw new NotSupportedException("Out of test scope");
		IXmlElement? IConverter<IXmlElement, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context) =>
			throw new NotSupportedException("Out of test scope");
		IXmlText? IConverter<IXmlText, IXmlNode>.Serialize(in object objectToSerialize, in ISerializerContext context) =>
			throw new NotSupportedException("Out of test scope");
		public object? Deserialize(in IXmlElement objectToDeserialize, in ISerializerContext<IXmlNode> context) =>
			throw new NotSupportedException("Out of test scope");
		public object? Deserialize(in IXmlAttribute objectToDeserialize, in ISerializerContext<IXmlNode> context) =>
			throw new NotSupportedException("Out of test scope");
		public object? Deserialize(in IXmlText objectToDeserialize, in ISerializerContext<IXmlNode> context) =>
			throw new NotSupportedException("Out of test scope");
	}
}
