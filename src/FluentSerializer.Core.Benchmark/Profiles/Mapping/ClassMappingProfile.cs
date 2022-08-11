using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Benchmark.Profiles.Mapping;

[MemoryDiagnoser]
public partial class ClassMappingProfile
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

	private static readonly IReadOnlyList<ClassMap> ClassMapsArray = GenerateClassMaps(PropertyMapsArray!).ToArray();
	private static readonly IReadOnlyList<ClassMap> ClassMapsList = GenerateClassMaps(PropertyMapsList!).ToList();
	private static readonly IReadOnlyList<PropertyMap> PropertyMapsArray = GeneratePropertyMaps.ToArray();
	private static readonly IReadOnlyList<PropertyMap> PropertyMapsList = GeneratePropertyMaps.ToList();

	[Benchmark]
	public IClassMap? GetNewClassMapArray()
	{
		var scanList = new ClassMapCollection(ClassMapsArray);

		return scanList.GetClassMapFor(typeof(TestClass), SerializerDirection.Serialize);
	}

	[Benchmark]
	public IClassMap? GetNewClassMapList()
	{
		var scanList = new ClassMapCollection(ClassMapsList);

		return scanList.GetClassMapFor(typeof(TestClass), SerializerDirection.Serialize);
	}

	[Benchmark]
	public IPropertyMap? GetNewPropertyMapArray()
	{
		var scanList = new PropertyMapCollection(PropertyMapsArray);

		return scanList.GetPropertyMapFor(
			typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
			SerializerDirection.Serialize);
	}

	[Benchmark]
	public IPropertyMap? GetNewPropertyMapList()
	{
		var scanList = new PropertyMapCollection(PropertyMapsList);

		return scanList.GetPropertyMapFor(
			typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
			SerializerDirection.Serialize);
	}
}

public class TestClass
{
	public bool TestValue { get; set; }
}