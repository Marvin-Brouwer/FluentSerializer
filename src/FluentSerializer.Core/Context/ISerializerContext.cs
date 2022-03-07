using System;
using System.Reflection;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Context;

/// <summary>
/// Current context for serializing data
/// </summary>
public interface ISerializerContext : ISerializerCoreContext, INamingContext
{
	/// <summary>
	/// The original propertyInfo used to define this mapping
	/// </summary>
	PropertyInfo Property { get; }
	/// <summary>
	/// The actual type of the property with resolved generics based on the current <see cref="ClassType"/>
	/// <br /><b>
	/// If the <see cref="Property"/> is of type <see cref="Nullable{T}"/> this will return the value of
	/// <see cref="Nullable.GetUnderlyingType"/>
	/// </b>
	/// </summary>
	Type PropertyType { get; }
	/// <summary>
	/// The type of the class(on the current level on nesting) passed to the serializer
	/// <br /><b>
	/// If the current class type is of type <see cref="Nullable{T}"/> this will return the value of
	/// <see cref="Nullable.GetUnderlyingType"/>
	/// </b>
	/// </summary>
	Type ClassType { get; }

	/// <summary>
	/// The <see cref="INamingStrategy"/> passed for the property currently being (de)serialized
	/// </summary>
	INamingStrategy NamingStrategy { get; }

	/// <summary>
	/// Find the <see cref="INamingStrategy"/>  for any property of the current <see cref="ClassType"/> if registered
	/// This can be useful when unpacking collections to a different data structure
	/// </summary>
	/// <param name="property"></param>
	/// <returns></returns>
	INamingStrategy? FindNamingStrategy(in PropertyInfo property);
}

/// <summary>
/// Current context for serializing data
/// </summary>
public interface ISerializerContext<out TDataNode> : ISerializerContext, ISerializerCoreContext<TDataNode>
	where TDataNode : IDataNode
{

	/// <summary>
	/// Get the parent node
	/// </summary>
	TDataNode? ParentNode { get; }
}