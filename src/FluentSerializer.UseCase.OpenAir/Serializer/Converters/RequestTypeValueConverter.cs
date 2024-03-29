using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

using System;
using System.Reflection;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters;

/// <summary>
/// The RequestTypeValueConverter is used to reflect out the element name of the data passed.
/// OpenAir requires this value to be matched exactly on the type attribute.
/// </summary>
public class RequestTypeValueConverter : IXmlConverter<IXmlAttribute>
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Serialize;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(string) == targetType;
	/// <inheritdoc />
#pragma warning disable CA1725 // Parameter names should match base declaration
	public object Deserialize(in IXmlAttribute attributeToDeserialize, in ISerializerContext<IXmlNode> context) => throw new NotSupportedException();
#pragma warning restore CA1725 // Parameter names should match base declaration

	/// <inheritdoc />
	public Guid ConverterId { get; } = typeof(RequestTypeValueConverter).GUID;

	/// <inheritdoc />
	public IXmlAttribute? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		// We know this to be true because of RequestObject<TModel>
		var classType = context.ClassType.GetTypeInfo().GenericTypeArguments[0];
		var classNamingStrategy = context.FindNamingStrategy(in classType);
		if (classNamingStrategy is null)
			throw new NotSupportedException($"Unable to find a NamingStrategy for '{classType.FullName}'");

		var elementTypeString = classNamingStrategy.SafeGetName(classType, context);
		var attributeName = context.NamingStrategy.SafeGetName(context.Property, context.PropertyType, context);

		return Attribute(in attributeName, in elementTypeString);
	}
}