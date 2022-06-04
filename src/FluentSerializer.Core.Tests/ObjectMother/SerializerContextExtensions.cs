using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Services;
using Moq;
using System;
using System.Reflection;

namespace FluentSerializer.Core.Tests.ObjectMother;

public static class SerializerContextMother
{
	internal class TestSerializerConfiguration : SerializerConfiguration {
		public static readonly TestSerializerConfiguration Default = new();
	}
	/// <summary>
	/// Create a minimal context mock for testing
	/// </summary>
	public static Mock<TSerializer> SetupDefault<TSerializer>(this Mock<TSerializer> contextMock)
		where TSerializer : class, ISerializer
	{
		contextMock
			.Setup(context => context.Configuration)
			.Returns(TestSerializerConfiguration.Default);

		return contextMock;
	}
	/// <summary>
	/// Create a minimal context mock for testing
	/// </summary>
	public static Mock<ISerializerContext<TDataNode>> SetupDefault<TDataNode, TSerializer>(
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
	/// Make the <see cref="ISerializerContext.PropertyType"/> return type of <paramref name="propertyType"/>
	/// </summary>
	public static Mock<ISerializerContext<TDataNode>> WithPropertyType<TDataNode>(
		this Mock<ISerializerContext<TDataNode>> contextMock, Type propertyType)
		where TDataNode : IDataNode
	{
		contextMock
			.Setup(context => context.PropertyType)
			.Returns(propertyType);

		return contextMock;
	}

	/// <summary>
	/// Make the <see cref="ISerializerContext.FindNamingStrategy"/> return <paramref name="namingStrategy"/>
	/// </summary>
	public static Mock<ISerializerContext<TDataNode>> WithFindNamingStrategy<TDataNode>(
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
	/// Make the <see cref="ISerializerContext.ParentNode"/> return <paramref name="parent"/>
	/// </summary>
	public static Mock<ISerializerContext<TDataNode>> WithParentNode<TDataNode>(
		this Mock<ISerializerContext<TDataNode>> contextMock, TDataNode parent)
		where TDataNode : IDataNode
	{
		contextMock
			.Setup(context => context.ParentNode)
			.Returns(parent);

		return contextMock;
	}

	/// <summary>
	/// Make the <see cref="ISerializerContext.NamingStrategy"/> return <paramref name="namingStrategy"/>
	/// </summary>
	public static Mock<ISerializerContext<TDataNode>> WithNamingStrategy<TDataNode>(
		this Mock<ISerializerContext<TDataNode>> contextMock, Func<INamingStrategy> namingStrategy)
		where TDataNode : IDataNode
	{
		contextMock
			.Setup(context => context.NamingStrategy)
			.Returns(namingStrategy);

		return contextMock;
	}

	/// <summary>
	/// Make the <see cref="ISerializerContext.NamingStrategy"/> return <paramref name="namingStrategy"/>
	/// </summary>
	public static Mock<ISerializerCoreContext<TDataNode>> WithSerializer<TDataNode, TSerializer>(
		this Mock<ISerializerCoreContext<TDataNode>> contextMock, TSerializer serializer)
		where TDataNode : IDataNode
		where TSerializer : ISerializer
	{
		contextMock
			.Setup(context => context.CurrentSerializer)
			.Returns(serializer);

		return contextMock;
	}

	/// <summary>
	/// Make the <see cref="ISerializerContext.NamingStrategy"/> return <paramref name="namingStrategy"/>
	/// </summary>
	public static Mock<ISerializerCoreContext<TDataNode>> WithSerializer<TDataNode, TSerializer>(
		this Mock<ISerializerCoreContext<TDataNode>> contextMock, IMock<TSerializer> serializer)
		where TDataNode : IDataNode
		where TSerializer : class, ISerializer
	{
		return contextMock.WithSerializer(serializer.Object);
	}

	/// <summary>
	/// Make the <see cref="ISerializerContext.NamingStrategy"/> return <paramref name="namingStrategy"/>
	/// </summary>
	public static Mock<ISerializerCoreContext<TDataNode>> WithAutoPathSegment<TDataNode>(
		this Mock<ISerializerCoreContext<TDataNode>> contextMock)
		where TDataNode : IDataNode
	{
		contextMock
			.Setup(context => context.WithPathSegment(It.Ref<PropertyInfo>.IsAny))
			.Returns((PropertyInfo _) => contextMock.Object);
		contextMock
			.Setup(context => context.WithPathSegment(It.Ref<Type>.IsAny))
			.Returns((Type _) => contextMock.Object);

		return contextMock;
	}
}