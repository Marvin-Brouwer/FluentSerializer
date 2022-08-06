using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;

namespace FluentSerializer.Core.Benchmark.Profiles.Mapping;

[MemoryDiagnoser]
public partial class ClassMappingProfile
{
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

	private static IEnumerable<ClassMap> GenerateClassMapsArray(IReadOnlyList<IPropertyMap> propertyMaps) =>
		Array.Empty<SerializerDirection>()
			.Concat(Enumerable.Repeat(SerializerDirection.Both, 200))
			.Concat(Enumerable.Repeat(SerializerDirection.Serialize, 100))
			.Concat(Enumerable.Repeat(SerializerDirection.Deserialize, 100))
			.Select(direction => new ClassMap(
				typeof(TestClass),
				direction,
				Names.Use.CamelCase,
				propertyMaps));

	private static readonly IReadOnlyList<PropertyMap> PropertyMapsArray = GeneratePropertyMaps.ToArray();
	private static readonly IReadOnlyList<ClassMap> ClassMapsArray = GenerateClassMapsArray(Array.Empty<IPropertyMap>()).ToArray();
	private static readonly IReadOnlyList<PropertyMap> PropertyMapsList = GeneratePropertyMaps.ToList();
	private static readonly IReadOnlyList<ClassMap> ClassMapsList = GenerateClassMapsArray(Array.Empty<IPropertyMap>()).ToList();

	[Benchmark(Baseline = true), BenchmarkCategory("CreateFullMap")]
	public ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration> CreateClassMap()
	{
		var classMaps = new List<IClassMap>();

		for (int classMapIteration = 0; classMapIteration < 500; classMapIteration++)
		{
			var propertyMaps = new List<IPropertyMap>();
			for (int propertyMapIteration = 0; propertyMapIteration < 500; propertyMapIteration++)
			{
				var propertyDirection = classMapIteration switch
				{
					< 200 => SerializerDirection.Both,
					< 300 => SerializerDirection.Serialize,
					_ => SerializerDirection.Deserialize
				};

				propertyMaps.Add(new PropertyMap(propertyDirection,
					typeof(TestClass),
					typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
					Names.Use.KebabCase, null));
			}

			var classDirection = classMapIteration switch
			{
				< 200 => SerializerDirection.Both,
				< 300 => SerializerDirection.Serialize,
				_ => SerializerDirection.Deserialize
			};
			classMaps.Add(
				new ClassMap(typeof(TestClass), classDirection,
					Names.Use.CamelCase, propertyMaps));
		}
		return new ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(
			classMaps);
	}

	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration> CreateClass_ReadOnlyCollectionBuilder()
	{
		var classMapBuilder = new ReadOnlyCollectionBuilder<IClassMap>();

		for (int classMapIteration = 0; classMapIteration < 500; classMapIteration++)
		{
			var propertyMapBuilder = new ReadOnlyCollectionBuilder<IPropertyMap>();
			for (int propertyMapIteration = 0; propertyMapIteration < 500; propertyMapIteration++)
			{
				var propertyDirection = classMapIteration switch
				{
					< 200 => SerializerDirection.Both,
					< 300 => SerializerDirection.Serialize,
					_ => SerializerDirection.Deserialize
				};

				propertyMapBuilder.Add(new PropertyMap(propertyDirection,
					typeof(TestClass),
					typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
					Names.Use.KebabCase, null));
			}

			var classDirection = classMapIteration switch
			{
				< 200 => SerializerDirection.Both,
				< 300 => SerializerDirection.Serialize,
				_ => SerializerDirection.Deserialize
			};
			classMapBuilder.Add(
				new ClassMap(typeof(TestClass), classDirection,
				Names.Use.CamelCase, propertyMapBuilder.ToReadOnlyCollection()));
		}
		return new ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(
			classMapBuilder.ToReadOnlyCollection());
	}

	[Benchmark(Baseline = true), BenchmarkCategory("GetClassMap")]
	public IClassMap? GetClassMapArray()
	{
		var scanList = new ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(
			ClassMapsArray);

		return scanList.Scan((typeof(TestClass), SerializerDirection.Serialize));
	}

	[Benchmark, BenchmarkCategory("GetClassMap")]
	public IClassMap? GetClassMapList()
	{
		var scanList = new ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(
			ClassMapsList);

		return scanList.Scan((typeof(TestClass), SerializerDirection.Serialize));
	}

	[Benchmark(Baseline = true), BenchmarkCategory("GetPropertyMap")]
	public IPropertyMap? GetPropertyMapArray()
	{
		var scanList = new PropertyMapScanList(PropertyMapsArray);

		return scanList.Scan(typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!);
	}

	[Benchmark, BenchmarkCategory("GetPropertyMap")]
	public IPropertyMap? GetPropertyMapList()
	{
		var scanList = new PropertyMapScanList(PropertyMapsList);

		return scanList.Scan(typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!);
	}
}

public class TestClass
{
	public bool TestValue { get; set; }
}