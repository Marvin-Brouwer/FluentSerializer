using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentSerializer.Json.Profiles;

/// <inheritdoc />
public sealed class JsonProfileBuilder<TModel> : IJsonProfileBuilder<TModel>
	where TModel : new()
{
	private readonly Func<INamingStrategy> _defaultNamingStrategy;
	private readonly List<IPropertyMap> _propertyMap;

	/// <inheritdoc cref="JsonProfileBuilder{TModel}" />
	public JsonProfileBuilder(in Func<INamingStrategy> defaultNamingStrategy, in List<IPropertyMap> propertyMap)
	{
		Guard.Against.Null(defaultNamingStrategy
#if NETSTANDARD
			, nameof(defaultNamingStrategy)
#endif
		);
		Guard.Against.Null(propertyMap
#if NETSTANDARD
			, nameof(propertyMap)
#endif
		);

		_defaultNamingStrategy = defaultNamingStrategy;
		_propertyMap = propertyMap;
	}

	/// <inheritdoc />
	public IJsonProfileBuilder<TModel> Property<TProperty>(
		in Expression<Func<TModel, TProperty>> propertySelector,
		in SerializerDirection direction = SerializerDirection.Both,
		in Func<INamingStrategy>? namingStrategy = null,
		in Func<IJsonConverter>? converter = null
	)
	{
		_propertyMap.Add(new PropertyMap(
			in direction,
			typeof(IJsonProperty),
			propertySelector.GetProperty(),
			namingStrategy ?? _defaultNamingStrategy,
			converter
		));

		return this;
	}
}