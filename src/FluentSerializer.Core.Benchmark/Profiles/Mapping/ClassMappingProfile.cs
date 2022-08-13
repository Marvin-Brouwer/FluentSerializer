using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Benchmark.Profiles.Mapping;

[MemoryDiagnoser]
public sealed class ClassMappingProfile
{
	private static IEnumerable<ClassMap> GenerateClassMaps(IReadOnlyList<IPropertyMap> propertyMaps) =>
		Array.Empty<SerializerDirection>()
			.Concat(Enumerable.Repeat(SerializerDirection.Both, 200))
			.Concat(Enumerable.Repeat(SerializerDirection.Serialize, 100))
			.Concat(Enumerable.Repeat(SerializerDirection.Deserialize, 100))
			.Select(direction => new ClassMap(
				typeof(TestClass),
				direction,
				Names.Use.CamelCase,
				propertyMaps));

	private static IEnumerable<PropertyMap> GeneratePropertyMaps =>
		Array.Empty<SerializerDirection>()
			.Concat(Enumerable.Repeat(SerializerDirection.Both, 200))
			.Concat(Enumerable.Repeat(SerializerDirection.Serialize, 100))
			.Concat(Enumerable.Repeat(SerializerDirection.Deserialize, 100))
			.Select(direction => new PropertyMap(
				direction,
				typeof(TestClass),
				typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
				Names.Use.KebabCase, null));

	private static readonly IReadOnlyList<PropertyMap> PropertyMapsArray = GeneratePropertyMaps.ToArray();
	private static readonly IReadOnlyList<ClassMap> ClassMapsArray = GenerateClassMaps(PropertyMapsArray).ToArray();

	[Benchmark]
	public IClassMap? GetClassMap()
	{
		var scanList = new ClassMapCollection(ClassMapsArray);

		return scanList.GetClassMapFor(typeof(TestClass), SerializerDirection.Serialize);
	}

	[Benchmark]
	public IPropertyMap? GetPropertyMap()
	{
		var scanList = new PropertyMapCollection(PropertyMapsArray);

		return scanList.GetPropertyMapFor(
			typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
			SerializerDirection.Serialize);
	}
}

public class TestClass
{
	public bool TestValue { get; set; }
}