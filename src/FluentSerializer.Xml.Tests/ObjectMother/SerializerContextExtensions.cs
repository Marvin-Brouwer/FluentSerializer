using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Services;
using FluentSerializer.Xml.DataNodes;
using Moq;
using System;
using System.Reflection;

namespace FluentSerializer.Xml.Tests.ObjectMother
{
	internal static class SerializerContextExtensions
	{
		/// <summary>
		/// TODO Move to shared
		/// Create a minimal context mock for testing
		/// </summary>
		internal static Mock<ISerializerContext<TDataNode>> SetupDefault<TDataNode, TSerializer>(
			this Mock<ISerializerContext<TDataNode>> contextMock,
			Mock<TSerializer> serializerMock)
			where TDataNode : IDataNode
			where TSerializer : class, ISerializer
		{
			contextMock
				.Setup(context => context.CurrentSerializer)
				.Returns(serializerMock.Object);

			return contextMock;
		}

		/// <summary>
		/// TODO Move to shared
		/// Make the <see cref="ISerializerContext.PropertyType"/> return type of <paramref name="propertyType"/>
		/// </summary>
		internal static Mock<ISerializerContext<TDataNode>> WithPropertyType<TDataNode>(
			this Mock<ISerializerContext<TDataNode>> contextMock, Type propertyType)
			where TDataNode : IDataNode
		{
			contextMock
				.Setup(context => context.PropertyType)
				.Returns(propertyType);

			return contextMock;
		}

		/// <summary>
		/// TODO Move to shared
		/// Make the <see cref="ISerializerContext.FindNamingStrategy"/> return <paramref name="namingStrategy"/>
		/// </summary>
		internal static Mock<ISerializerContext<TDataNode>> WithFindNamingStrategy<TDataNode>(
			this Mock<ISerializerContext<TDataNode>> contextMock, Func<INamingStrategy> namingStrategy)
			where TDataNode : IDataNode
		{
			contextMock
				.Setup(context => context.FindNamingStrategy(in It.Ref<Type>.IsAny))
				.Returns(namingStrategy);
			contextMock
				.Setup(context => context.FindNamingStrategy(in It.Ref<PropertyInfo>.IsAny))
				.Returns(namingStrategy);

			return contextMock;
		}

		/// <summary>
		/// Make the <see cref="ISerializerContext.PropertyType"/> return type of <typeparamref name="TProperty"/>
		/// </summary>
		internal static Mock<ISerializerContext<IXmlNode>> WithPropertyType<TProperty>(this Mock<ISerializerContext<IXmlNode>> contextMock) =>
			contextMock.WithPropertyType<IXmlNode>(typeof(TProperty));

		/// <summary>
		/// TODO Move to shared
		/// Make the <see cref="ISerializerContext.ParentNode"/> return <paramref name="parent"/>
		/// </summary>
		internal static Mock<ISerializerContext<TDataNode>> WithParentNode<TDataNode>(
			this Mock<ISerializerContext<TDataNode>> contextMock, TDataNode parent)
			where TDataNode : IDataNode
		{
			contextMock
				.Setup(context => context.ParentNode)
				.Returns(parent);

			return contextMock;
		}

		/// <summary>
		/// TODO Move to shared
		/// Make the <see cref="ISerializerContext.NamingStrategy"/> return <paramref name="namingStrategy"/>
		/// </summary>
		internal static Mock<ISerializerContext<TDataNode>> WithNamingStrategy<TDataNode>(
			this Mock<ISerializerContext<TDataNode>> contextMock, Func<INamingStrategy> namingStrategy)
			where TDataNode : IDataNode
		{
			contextMock
				.Setup(context => context.NamingStrategy)
				.Returns(namingStrategy);

			return contextMock;
		}
	}
}
