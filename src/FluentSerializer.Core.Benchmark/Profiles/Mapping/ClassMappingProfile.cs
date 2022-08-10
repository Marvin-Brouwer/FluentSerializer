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

	private static readonly IReadOnlyList<ClassMap> ClassMapsArray = GenerateClassMaps(Array.Empty<IPropertyMap>()).ToArray();
	private static readonly IReadOnlyList<ClassMap> ClassMapsList = GenerateClassMaps(Array.Empty<IPropertyMap>()).ToList();
	private static readonly IReadOnlyList<PropertyMap> PropertyMapsArray = GeneratePropertyMaps.ToArray();
	private static readonly IReadOnlyList<PropertyMap> PropertyMapsList = GeneratePropertyMaps.ToList();

	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapCollection CreateClass_ClassMapCollection_List()
	{
		var classMapBuilder = new List<IClassMap>();

		for (int classMapIteration = 0; classMapIteration < 500; classMapIteration++)
		{
			var propertyMapBuilder = new List<IPropertyMap>();
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
					Names.Use.CamelCase, propertyMapBuilder));
		}

		return new ClassMapCollection(classMapBuilder);
	}

	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapCollection CreateClass_ClassMapCollection_Array()
	{
		var classMapBuilder = new IClassMap[500];

		for (int classMapIteration = 0; classMapIteration < 500; classMapIteration++)
		{
			var propertyMapBuilder = new IPropertyMap[500];
			for (int propertyMapIteration = 0; propertyMapIteration < 500; propertyMapIteration++)
			{
				var propertyDirection = classMapIteration switch
				{
					< 200 => SerializerDirection.Both,
					< 300 => SerializerDirection.Serialize,
					_ => SerializerDirection.Deserialize
				};

				propertyMapBuilder[propertyMapIteration] = new PropertyMap(propertyDirection,
					typeof(TestClass),
					typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
					Names.Use.KebabCase, null);
			}

			var classDirection = classMapIteration switch
			{
				< 200 => SerializerDirection.Both,
				< 300 => SerializerDirection.Serialize,
				_ => SerializerDirection.Deserialize
			};
			classMapBuilder[classMapIteration] =
				new ClassMap(typeof(TestClass), classDirection,
					Names.Use.CamelCase, propertyMapBuilder);
		}

		return new ClassMapCollection(classMapBuilder);
	}

	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapCollection CreateClass_ClassMapCollection_ReadOnlyCollectionBuilder()
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

		return new ClassMapCollection(classMapBuilder.ToReadOnlyCollection());
	}

	[Benchmark, BenchmarkCategory("GetClassMap")]
	public IClassMap? GetNewClassMapArray()
	{
		var scanList = new ClassMapCollection(ClassMapsArray);

		return scanList.GetClassMapFor(typeof(TestClass), SerializerDirection.Serialize);
	}

	[Benchmark, BenchmarkCategory("GetClassMap")]
	public IClassMap? GetNewClassMapList()
	{
		var scanList = new ClassMapCollection(ClassMapsList);

		return scanList.GetClassMapFor(typeof(TestClass), SerializerDirection.Serialize);
	}

	[Benchmark, BenchmarkCategory("GetPropertyMap")]
	public IPropertyMap? GetNewPropertyMapArray()
	{
		var scanList = new PropertyMapCollection(PropertyMapsArray);

		return scanList.GetPropertyMapFor(
			typeof(TestClass).GetProperty(nameof(TestClass.TestValue))!,
			SerializerDirection.Serialize);
	}

	[Benchmark, BenchmarkCategory("GetPropertyMap")]
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