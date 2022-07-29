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
public class ClassMappingProfile
{
	private static IReadOnlyList<PropertyMap> GeneratePropertyMaps =>
		Enumerable.Repeat(new PropertyMap(SerializerDirection.Both,
			typeof(TestClass),
			typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
			Names.Use.KebabCase, null), 5000).ToArray();

	private static IReadOnlyList<ClassMap> GenerateClassMaps(IReadOnlyList<IPropertyMap> propertyMaps) =>
		Enumerable.Repeat(new ClassMap(typeof(TestClass), SerializerDirection.Both,
			Names.Use.CamelCase, propertyMaps), 20000).ToArray();

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

		for (int classMapIteration = 0; classMapIteration < 20000; classMapIteration++)
		{
			var propertyMapBuilder = new ReadOnlyCollectionBuilder<IPropertyMap>();
			for (int propertyMapIteration = 0; propertyMapIteration < 5000; propertyMapIteration++)
			{
				propertyMapBuilder.Add(new PropertyMap(SerializerDirection.Both,
					typeof(TestClass),
					typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
					Names.Use.KebabCase, null));
			}
			classMapBuilder.Add(
				new ClassMap(typeof(TestClass), SerializerDirection.Both,
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