using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.DataNodes;

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
		Guard.Against.Null(defaultNamingStrategy, nameof(defaultNamingStrategy));
		Guard.Against.Null(propertyMap, nameof(propertyMap));

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