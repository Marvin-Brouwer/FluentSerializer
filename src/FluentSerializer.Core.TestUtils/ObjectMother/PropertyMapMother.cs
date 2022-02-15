using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.TestUtils.ObjectMother
{
	public static class PropertyMapMother
	{
		/// <summary>
		/// Configure the mock of <see cref="IScanList{TScanBy, TScanFor}"/> for properties
		/// to return null on scan and no items on enumeration
		/// </summary>
		public static Mock<IScanList<PropertyInfo, IPropertyMap>> WithoutProppertyMapping(
			this Mock<IScanList<PropertyInfo, IPropertyMap>> propertyMapMock)
		{
			propertyMapMock
				.Setup(propertyMap => propertyMap.GetEnumerator())
				.Returns(new List<IPropertyMap> (0).GetEnumerator());
			propertyMapMock
				.Setup(propertyMap => propertyMap.Scan(It.IsAny<PropertyInfo>()))
				.Returns((IPropertyMap?)null);

			return propertyMapMock;
		}

		/// <summary>
		/// Configure the mock of <see cref="IScanList{TScanBy, TScanFor}"/> for properties
		/// to return <paramref name="propertyMapping"/> on scan and enumeration
		/// </summary>
		public static Mock<IScanList<PropertyInfo, IPropertyMap>> WithProppertyMapping(
			this Mock<IScanList<PropertyInfo, IPropertyMap>> propertyMapMock, IPropertyMap propertyMapping)
		{
			propertyMapMock
				.Setup(propertyMap => propertyMap.GetEnumerator())
				.Returns(new List<IPropertyMap> { propertyMapping }.GetEnumerator());
			propertyMapMock
				.Setup(propertyMap => propertyMap.Scan(It.IsAny<PropertyInfo>()))
				.Returns(propertyMapping);

			return propertyMapMock;
		}

		/// <summary>
		/// Create a simple representation of a <see cref="PropertyMap"/>
		/// </summary>
		/// <remarks>
		/// <see cref="INamingStrategy"/> is set to <see cref="Names.Use.PascalCase"/> because this essentially is a one-to-one mapping
		/// when using C# coding conventions so it saves us writing a TestNamingStrategy.
		/// </remarks>
		public static Mock<IScanList<PropertyInfo, IPropertyMap>> WithBasicProppertyMapping(
			this Mock<IScanList<PropertyInfo, IPropertyMap>> propertyMapMock,
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
}
