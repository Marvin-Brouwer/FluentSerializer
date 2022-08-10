using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;

using Moq;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Tests.ObjectMother;

public static class PropertyMapMother
{
	/// <summary>
	/// Configure the mock of <see cref="IScanList{TScanBy, TScanFor}"/> for properties
	/// to return null on scan and no items on enumeration
	/// </summary>
	public static Mock<IPropertyMapCollection> WithoutProppertyMapping(
		this Mock<IPropertyMapCollection> propertyMapMock)
	{
		propertyMapMock
			.Setup(propertyMap => propertyMap.GetAllPropertyMaps(It.Ref<SerializerDirection>.IsAny))
			.Returns(Array.Empty<IPropertyMap>());
		propertyMapMock
			.Setup(propertyMap => propertyMap.GetPropertyMapFor(It.Ref<PropertyInfo>.IsAny))
			.Returns((IPropertyMap?)null);
		propertyMapMock
			.Setup(propertyMap => propertyMap.GetPropertyMapFor(It.Ref<PropertyInfo>.IsAny, It.Ref<SerializerDirection>.IsAny))
			.Returns((IPropertyMap?)null);

		return propertyMapMock;
	}

	/// <summary>
	/// Configure the mock of <see cref="IScanList{TScanBy, TScanFor}"/> for properties
	/// to return <paramref name="propertyMapping"/> on scan and enumeration
	/// </summary>
	public static Mock<IPropertyMapCollection> WithProppertyMapping(
		this Mock<IPropertyMapCollection> propertyMapMock, IPropertyMap propertyMapping)
	{
		propertyMapMock
			.Setup(propertyMap => propertyMap.GetAllPropertyMaps(It.Ref<SerializerDirection>.IsAny))
			.Returns(new List<IPropertyMap> { propertyMapping });
		propertyMapMock
			.Setup(propertyMap => propertyMap.GetPropertyMapFor(It.Ref<PropertyInfo>.IsAny))
			.Returns((in PropertyInfo propertyInfoToCheck) => propertyInfoToCheck == propertyMapping.Property ? propertyMapping : null);
		propertyMapMock
			.Setup(propertyMap => propertyMap.GetPropertyMapFor(It.Ref<PropertyInfo>.IsAny, It.Ref<SerializerDirection>.IsAny))
			.Returns((in PropertyInfo propertyInfoToCheck, in SerializerDirection _) => propertyInfoToCheck == propertyMapping.Property ? propertyMapping : null);

		return propertyMapMock;
	}

	/// <summary>
	/// Create a simple representation of a <see cref="PropertyMap"/>
	/// </summary>
	/// <remarks>
	/// <see cref="Naming.NamingStrategies.INamingStrategy"/> is set to <see cref="Names.Use.PascalCase"/> because this essentially is a one-to-one mapping
	/// when using C# coding conventions so it saves us writing a TestNamingStrategy.
	/// </remarks>
	public static Mock<IPropertyMapCollection> WithBasicProppertyMapping(
		this Mock<IPropertyMapCollection> propertyMapMock,
		SerializerDirection direction, Type containerType, PropertyInfo targetProperty,
		Func<IConverter> assignedConverter)
	{
		var propertyMapping = new PropertyMap(direction,
			containerType,
			targetProperty, Names.Use.PascalCase,
			assignedConverter);

		return propertyMapMock
			.WithProppertyMapping(propertyMapping);
	}
}