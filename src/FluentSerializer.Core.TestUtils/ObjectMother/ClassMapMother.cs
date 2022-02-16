using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using Moq;
using System;
using System.Reflection;

namespace FluentSerializer.Core.TestUtils.ObjectMother
{
	public static class ClassMapMother
	{
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
		/// Configure the property mappings to be empty
		/// </summary>
		public static Mock<IClassMap> WithoutPropertyMaps(this Mock<IClassMap> classMapMock)
		{
			var propertyMapsMock = new Mock<IScanList<PropertyInfo, IPropertyMap>>()
				.WithoutProppertyMapping();

			return classMapMock
				.WithPropertyMaps(propertyMapsMock.Object);
		}

		/// <summary>
		/// Configure the property mappings to <paramref name="propertyMapping"/>
		/// </summary>
		public static Mock<IClassMap> WithPropertyMaps(this Mock<IClassMap> classMapMock, IScanList<PropertyInfo, IPropertyMap> propertyMaps)
		{
			classMapMock
				.Setup(classMap => classMap.PropertyMaps)
				.Returns(propertyMaps);

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
			var propertyMap = new Mock<IScanList<PropertyInfo, IPropertyMap>>()
				.WithBasicProppertyMapping(direction, containerType, targetProperty, assignedConverter);

			return classMapMock
				//.WithPropertyType(targetProperty.PropertyType)
				.WithPropertyMaps(propertyMap.Object);
		}
	}
}
