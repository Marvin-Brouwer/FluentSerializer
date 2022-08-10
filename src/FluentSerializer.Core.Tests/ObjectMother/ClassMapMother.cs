using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

using Moq;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Tests.ObjectMother;

public static class ClassMapMother
{
	/// <summary>
	/// Configure the ClassMap with defaults
	/// </summary>
	public static Mock<IClassMap> WithDefaults(this Mock<IClassMap> classMapMock)
	{
		return classMapMock
			.WithSerializerDirection(SerializerDirection.Both)
			.WithNamingStrategy(Names.Use.PascalCase);
	}

	/// <summary>
	/// Configure the ClassMap with <paramref name="classType"/>
	/// </summary>
	public static Mock<IClassMap> WithClassType(this Mock<IClassMap> classMapMock, Type classType)
	{
		classMapMock
			.Setup(classMap => classMap.ClassType)
			.Returns(classType);

		return classMapMock;
	}

	/// <summary>
	/// Configure the ClassMap with <paramref name="serializerDirection"/>
	/// </summary>
	public static Mock<IClassMap> WithSerializerDirection(this Mock<IClassMap> classMapMock, SerializerDirection serializerDirection)
	{
		classMapMock
			.Setup(classMap => classMap.Direction)
			.Returns(serializerDirection);

		return classMapMock;
	}

	/// <summary>
	/// Configure the ClassMap with <paramref name="namingStrategy"/>
	/// </summary>
	public static Mock<IClassMap> WithNamingStrategy(this Mock<IClassMap> classMapMock, Func<INamingStrategy> namingStrategy)
	{
		classMapMock
			.Setup(classMap => classMap.NamingStrategy)
			.Returns(namingStrategy);

		return classMapMock;
	}

	/// <summary>
	/// Setup the mock of <see cref="IClassMapCollection"/> to return <paramref name="mapping"/> on GetClassMapFor
	/// </summary>
	public static Mock<IClassMapCollection> WithClassMap(this Mock<IClassMapCollection> scanListMock, Type type, IClassMap mapping)
	{
		scanListMock
			.Setup(list => list.GetClassMapFor(It.Ref<Type>.IsAny))
			.Returns((Type typeRequested) => typeRequested == type ? mapping : null);

		scanListMock
			.Setup(list => list.GetClassMapFor(It.Ref<Type>.IsAny, It.Ref<SerializerDirection>.IsAny))
			.Returns((Type typeRequested, SerializerDirection _) => typeRequested == type ? mapping : null);

		return scanListMock;
	}

	/// <summary>
	/// Setup the mock of <see cref="IClassMapCollection"/> to return <paramref name="mappingMock"/>'s object on GetClassMapFor
	/// </summary>
	public static Mock<IClassMapCollection> WithClassMap(this Mock<IClassMapCollection> scanListMock, Type type, IMock<IClassMap> mappingMock)
	{
		return scanListMock
			.WithClassMap(type, mappingMock.Object);
	}

	/// <summary>
	/// Configure the property mappings to be empty
	/// </summary>
	public static Mock<IClassMap> WithoutPropertyMaps(this Mock<IClassMap> classMapMock)
	{
		var propertyMapsMock = new Mock<IPropertyMapCollection>()
			.WithoutProppertyMapping();

		return classMapMock
			.WithPropertyMaps(propertyMapsMock.Object);
	}

	/// <summary>
	/// Configure the property mappings to <paramref name="propertyMapping"/>
	/// </summary>
	public static Mock<IClassMap> WithPropertyMaps(this Mock<IClassMap> classMapMock, IPropertyMapCollection propertyMaps)
	{
		classMapMock
			.SetupGet(classMap => classMap.PropertyMapCollection)
			.Returns(propertyMaps);
		classMapMock
			.Setup(classMap => classMap.GetAllPropertyMaps(It.Ref<SerializerDirection>.IsAny))
			.Returns((in SerializerDirection direction) => propertyMaps.GetAllPropertyMaps(in direction));
		classMapMock
			.Setup(classMap => classMap.GetPropertyMapFor(It.Ref<PropertyInfo>.IsAny))
			.Returns((in PropertyInfo info) => propertyMaps.GetPropertyMapFor(in info));
		classMapMock
			.Setup(classMap => classMap.GetPropertyMapFor(It.Ref<PropertyInfo>.IsAny, It.Ref<SerializerDirection>.IsAny))
			.Returns((in PropertyInfo info, in SerializerDirection direction) => propertyMaps.GetPropertyMapFor(in info, in direction));

		return classMapMock;
	}

	/// <summary>
	/// Create a class map with a single simple representation of a <see cref="PropertyMap"/>
	/// </summary>
	/// <inheritdoc cref="PropertyMapMother.WithBasicProppertyMapping"/>
	public static Mock<IClassMap> WithBasicProppertyMapping(
		this Mock<IClassMap> classMapMock,
		SerializerDirection direction, Type containerType, PropertyInfo targetProperty, Func<IConverter> assignedConverter)
	{
		var propertyMap = new Mock<IPropertyMapCollection>()
			.WithBasicProppertyMapping(direction, containerType, targetProperty, assignedConverter);

		return classMapMock
			.WithPropertyMaps(propertyMap.Object);
	}

	/// <summary>
	/// Create a class map with a single simple representation of a <see cref="PropertyMap"/>
	/// </summary>
	/// <inheritdoc cref="PropertyMapMother.WithBasicProppertyMapping"/>
	public static IReadOnlyCollection<IClassMap> ToCollection(
		this Mock<IClassMap> classMapMock)
	{
		return new List<IClassMap> { classMapMock.Object };
	}
}