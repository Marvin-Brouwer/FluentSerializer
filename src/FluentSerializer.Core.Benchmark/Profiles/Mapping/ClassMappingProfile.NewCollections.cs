using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;

namespace FluentSerializer.Core.Benchmark.Profiles.Mapping;

public partial class ClassMappingProfile
{
	private static IEnumerable<NewClassMap> GenerateNewClassMaps(IReadOnlyList<IPropertyMap> propertyMaps) =>
		Array.Empty<SerializerDirection>()
			.Concat(Enumerable.Repeat(SerializerDirection.Both, 200))
			.Concat(Enumerable.Repeat(SerializerDirection.Serialize, 100))
			.Concat(Enumerable.Repeat(SerializerDirection.Deserialize, 100))
			.Select(direction => new NewClassMap(
				typeof(TestClass),
				direction,
				Names.Use.CamelCase,
				propertyMaps));

	private static readonly IReadOnlyList<NewClassMap> NewClassMapsArray = GenerateNewClassMaps(Array.Empty<IPropertyMap>()).ToArray();
	private static readonly IReadOnlyList<NewClassMap> NewClassMapsList = GenerateNewClassMaps(Array.Empty<IPropertyMap>()).ToList();
	
	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapCollection CreateClass_ClassMapCollection_List()
	{
		var classMapBuilder = new List<INewClassMap>();

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
				new NewClassMap(typeof(TestClass), classDirection,
					Names.Use.CamelCase, propertyMapBuilder));
		}

		return new ClassMapCollection(classMapBuilder);
	}

	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapCollection CreateClass_ClassMapCollection_Array()
	{
		var classMapBuilder = new INewClassMap[500];

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
				new NewClassMap(typeof(TestClass), classDirection,
					Names.Use.CamelCase, propertyMapBuilder);
		}

		return new ClassMapCollection(classMapBuilder);
	}

	[Benchmark, BenchmarkCategory("CreateFullMap")]
	public ClassMapCollection CreateClass_ClassMapCollection_ReadOnlyCollectionBuilder()
	{
		var classMapBuilder = new ReadOnlyCollectionBuilder<INewClassMap>();

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
				new NewClassMap(typeof(TestClass), classDirection,
					Names.Use.CamelCase, propertyMapBuilder.ToReadOnlyCollection()));
		}

		return new ClassMapCollection(classMapBuilder.ToReadOnlyCollection());
	}

	[Benchmark, BenchmarkCategory("GetClassMap")]
	public INewClassMap? GetNewClassMapArray()
	{
		var scanList = new ClassMapCollection(NewClassMapsArray);

		return scanList.GetClassMapFor(typeof(TestClass), SerializerDirection.Serialize);
	}

	[Benchmark, BenchmarkCategory("GetClassMap")]
	public INewClassMap? GetNewClassMapList()
	{
		var scanList = new ClassMapCollection(NewClassMapsList);

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