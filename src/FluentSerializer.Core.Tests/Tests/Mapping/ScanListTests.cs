using FluentAssertions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Mapping;

public sealed class ScanListTests
{
	private const SerializerDirection TestDirection = SerializerDirection.Serialize;
	private static readonly Func<INamingStrategy> TestNames = Names.Are("Test");
	private static readonly PropertyInfo CorrectProperty = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;

	private static readonly List<IPropertyMap> PropertyMaps = new ()
	{
		new PropertyMap(TestDirection, typeof(bool), CorrectProperty, TestNames, null)
	};
	private static readonly List<ClassMap> ClassMaps = new ()
	{
		new ClassMap(typeof(bool), TestDirection, TestNames, PropertyMaps)
	};

	[Fact,
		Trait("Category", "UnitTest")]
	public void ClassMapScan_NoMatch_ReturnsNone()
	{
		// Arrange
		var input = (typeof(int), SerializerDirection.Deserialize);
		var sut = new ClassMapScanList<ISerializerProfile>(ClassMaps);

		// Act
		var result = sut.Scan(input);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ClassMapScan_Match_ReturnsClassMap()
	{
		// Arrange
		var input = (typeof(bool), SerializerDirection.Serialize);
		var sut = new ClassMapScanList<ISerializerProfile>(ClassMaps);

		// Act
		var result = sut.Scan(input);

		// Assert
		result.Should().NotBeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void PropertyMapScan_NoMatch_ReturnsNone()
	{
		// Arrange
		var input = typeof(TestClass).GetProperty(nameof(TestClass.Name))!;
		var sut = new PropertyMapScanList(PropertyMaps);

		// Act
		var result = sut.Scan(input);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void PropertyMapScan_Match_ReturnsClassMap()
	{
		// Arrange
		var input = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;
		var sut = new PropertyMapScanList(PropertyMaps);

		// Act
		var result = sut.Scan(input);

		// Assert
		result.Should().NotBeNull();
	}

	private sealed  class TestClass {
		public int Id { get; set; }
		public string Name { get; set; } = default!;
	}
}