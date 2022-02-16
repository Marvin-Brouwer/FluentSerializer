using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Tests.ObjectMother;
using Moq;
using System;
using System.Reflection;

namespace FluentSerializer.Json.Tests.ObjectMother
{
	public static class PropertyMapMother
	{
		/// <remarks>
		/// <see cref="INamingStrategy"/> is set to <see cref="Names.Use.PascalCase"/> because this essentially is a one-to-one mapping
		/// when using C# coding conventions so it saves us writing a TestNamingStrategy.<br/>
		/// This mapping has a custom converter of <see cref="ConvertibleConverter"/> because that makes mocking a lot easier,
		/// the part where the serializer looks up a matching converter can be tested in isolation.
		/// </remarks>
		/// <inheritdoc cref="Core.Tests.ObjectMother.PropertyMapMother.WithBasicProppertyMapping"/>
		public static Mock<IScanList<PropertyInfo, IPropertyMap>> WithBasicProppertyMapping(
			this Mock<IScanList<PropertyInfo, IPropertyMap>> propertyMapMock,
			SerializerDirection direction, Type containerType, PropertyInfo targetProperty)
		{
			return propertyMapMock.WithBasicProppertyMapping(
				direction, containerType, targetProperty, ObjectMotherConstants.TestConverter);
		}
	}
}
