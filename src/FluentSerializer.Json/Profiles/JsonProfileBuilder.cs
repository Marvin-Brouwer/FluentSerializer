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

	/// <inheritdoc />
	public JsonProfileBuilder(Func<INamingStrategy> defaultNamingStrategy, List<IPropertyMap> propertyMap)
	{
		Guard.Against.Null(defaultNamingStrategy, nameof(defaultNamingStrategy));
		Guard.Against.Null(propertyMap, nameof(propertyMap));

		_defaultNamingStrategy = defaultNamingStrategy;
		_propertyMap = propertyMap;
	}

	/// <inheritdoc />
	public IJsonProfileBuilder<TModel> Property<TProperty>(
		Expression<Func<TModel, TProperty>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null,
		Func<IJsonConverter>? converter = null
	)
	{
		_propertyMap.Add(new PropertyMap(
			direction,
			typeof(IJsonProperty),
			propertySelector.GetProperty(),
			namingStrategy ?? _defaultNamingStrategy,
			converter
		));

		return this;
	}
}