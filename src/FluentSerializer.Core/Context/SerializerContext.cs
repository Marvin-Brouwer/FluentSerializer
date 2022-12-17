using FluentSerializer.Core.Converting;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Services;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Context;

/// <inheritdoc cref="ISerializerContext"/>
public class SerializerContext : ISerializerContext
{
	private readonly IPropertyMapCollection _propertyMappings;

	private readonly INamingContext _namingContext;

	private readonly ISerializerCoreContext _coreContext;

	/// <inheritdoc />
	public PropertyInfo Property { get; }
	/// <inheritdoc />
	public Type PropertyType { get; }
	/// <inheritdoc />
	public Type ClassType { get; }
	/// <inheritdoc />
	public INamingStrategy NamingStrategy  { get; }

	/// <inheritdoc />
	public ISerializer CurrentSerializer => _coreContext.CurrentSerializer;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Path => _coreContext.Path;

	/// <inheritdoc cref="ISerializerContext"/>
	public SerializerContext(
		in ISerializerCoreContext coreContext,
		in PropertyInfo property, in Type propertyType, in Type classType,
		in INamingStrategy namingStrategy,
		in IPropertyMapCollection propertyMappings,
		in IClassMapCollection classMappings)
	{
		_propertyMappings = propertyMappings;

		Property = property;
		PropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
		ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
		NamingStrategy = namingStrategy;

		_coreContext = coreContext;
		_namingContext = new NamingContext(classMappings);
	}

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public IDictionary<string, TValue> Data<TSerializer, TValue>()
		where TSerializer : IConverter => _coreContext.Data<TSerializer, TValue>();

	/// <inheritdoc />
#if NET6_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public INamingStrategy? FindNamingStrategy(in PropertyInfo propertyInfo) => NamingContext.FindNamingStrategy(in _propertyMappings, propertyInfo);

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public INamingStrategy? FindNamingStrategy(in Type classType, in PropertyInfo propertyInfo) => _namingContext.FindNamingStrategy(classType, propertyInfo);

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public INamingStrategy? FindNamingStrategy(in Type type) => _namingContext.FindNamingStrategy(type);

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public ISerializerCoreContext WithPathSegment(in PropertyInfo propertyInfo) => _coreContext.WithPathSegment(propertyInfo);

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public ISerializerCoreContext WithPathSegment(in Type type) => _coreContext.WithPathSegment(type);

	/// <inheritdoc />
	bool ISerializerCoreContext.TryAddReference(in object? instance) => _coreContext.TryAddReference(in instance);

	/// <inheritdoc />
	bool ISerializerCoreContext.ContainsReference(in object? instance) => _coreContext.ContainsReference(in instance);
}

/// <inheritdoc cref="ISerializerContext"/>
public sealed class SerializerContext<TSerialContainer> : SerializerContext, ISerializerContext<TSerialContainer>
	where TSerialContainer : IDataNode
{
	private readonly ISerializerCoreContext<TSerialContainer> _coreContext;

	/// <inheritdoc />
	public SerializerContext(
		in ISerializerCoreContext<TSerialContainer> coreContext,
		in PropertyInfo property, in Type propertyType, in Type classType, in INamingStrategy namingStrategy,
		in IPropertyMapCollection propertyMappings,
		in IClassMapCollection classMappings)
		: base(coreContext, in property, in propertyType, in classType, in namingStrategy,
			in propertyMappings, in classMappings)
	{
		_coreContext = coreContext;
	}

	/// <inheritdoc />
	public TSerialContainer RootNode => _coreContext.RootNode;

	/// <inheritdoc />
	public TSerialContainer? ParentNode { get; init; }

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public new ISerializerCoreContext<TSerialContainer> WithPathSegment(in PropertyInfo propertyInfo) => _coreContext.WithPathSegment(propertyInfo);

	/// <inheritdoc />
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public new ISerializerCoreContext<TSerialContainer> WithPathSegment(in Type type) => _coreContext.WithPathSegment(type);
}