using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Profiles;

namespace FluentSerializer.Core.Benchmark.Profiles;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public partial class ClassMappingProfile
{
	private static IReadOnlyList<PropertyMap> GeneratePropertyMaps =>
		Array.Empty<SerializerDirection>()
			.Concat(Enumerable.Repeat(SerializerDirection.Both, 200))
			.Concat(Enumerable.Repeat(SerializerDirection.Serialize, 100))
			.Concat(Enumerable.Repeat(SerializerDirection.Deserialize, 100))
			.Select(direction => new PropertyMap(
				direction,
				typeof(TestClass),
				typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
				Names.Use.KebabCase, null))
			.ToArray();

	private static IReadOnlyList<ClassMap> GenerateClassMaps(IReadOnlyList<IPropertyMap> propertyMaps) =>
		Array.Empty<SerializerDirection>()
			.Concat(Enumerable.Repeat(SerializerDirection.Both, 200))
			.Concat(Enumerable.Repeat(SerializerDirection.Serialize, 100))
			.Concat(Enumerable.Repeat(SerializerDirection.Deserialize, 100))
			.Select(direction => new ClassMap(
				typeof(TestClass),
				direction,
				Names.Use.CamelCase,
				propertyMaps))
			.ToArray();

	private static readonly IReadOnlyList<PropertyMap> PropertyMaps = GeneratePropertyMaps;
	private static readonly IReadOnlyList<ClassMap> ClassMaps = GenerateClassMaps(Array.Empty<IPropertyMap>());

	[Benchmark(Baseline = true), BenchmarkCategory("CreateFullMap")]
	public ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration> CreateClassMap()
	{
		return new ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(
			GenerateClassMaps(GeneratePropertyMaps));
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
	public IClassMap? GetClassMap()
	{
		var scanList = new ClassMapScanList<ISerializerProfile<ISerializerConfiguration>, ISerializerConfiguration>(
			ClassMaps);

		return scanList.Scan((typeof(TestClass), SerializerDirection.Serialize));
	}


	[Benchmark(Baseline = true), BenchmarkCategory("GetPropertyMap")]
	public IPropertyMap? GetPropertyMap()
	{
		var scanList = new PropertyMapScanList(PropertyMaps);


		return scanList.Scan(typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!);
	}
}

public class TestClass
{
	public bool TestValue { get; set; }
}