using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentSerializer.Xml.Profiles;

/// <inheritdoc />
public sealed class XmlProfileBuilder<TModel> : IXmlProfileBuilder<TModel>
	where TModel : new()
{
	private readonly Func<INamingStrategy> _defaultNamingStrategy;
	private readonly List<IPropertyMap> _propertyMap;

	/// <inheritdoc cref="XmlProfileBuilder{TMode}" />
	public XmlProfileBuilder(in Func<INamingStrategy> defaultNamingStrategy, in List<IPropertyMap> propertyMap)
	{
		Guard.Against.Null(defaultNamingStrategy, nameof(defaultNamingStrategy));
		Guard.Against.Null(propertyMap, nameof(propertyMap));

		_defaultNamingStrategy = defaultNamingStrategy;
		_propertyMap = propertyMap;
	}

	/// <inheritdoc />
	public IXmlProfileBuilder<TModel> Attribute<TAttribute>(
		in Expression<Func<TModel, TAttribute>> propertySelector,
		in SerializerDirection direction = SerializerDirection.Both,
		in Func<INamingStrategy>? namingStrategy = null,
		in Func<IXmlConverter<IXmlAttribute>>? converter = null
	)
	{
		_propertyMap.Add(new PropertyMap(
			in direction,
			typeof(IXmlAttribute),
			propertySelector.GetProperty(),
			namingStrategy ?? _defaultNamingStrategy,
			converter
		));

		return this;
	}

	/// <inheritdoc />
	public IXmlProfileBuilder<TModel> Child<TElement>(
		in Expression<Func<TModel, TElement>> propertySelector,
		in SerializerDirection direction = SerializerDirection.Both,
		in Func<INamingStrategy>? namingStrategy = null,
		in Func<IXmlConverter<IXmlElement>>? converter = null
	)
	{
		_propertyMap.Add(new PropertyMap(
			in direction,
			typeof(IXmlElement),
			propertySelector.GetProperty(),
			namingStrategy ?? _defaultNamingStrategy,
			converter
		));

		return this;
	}

	/// <remarks>
	/// XML Elements can only have one text node so this should be set last and doesn't return a <see cref="XmlProfileBuilder{TModel}"/>
	/// </remarks>
	public void Text<TText>(
		in Expression<Func<TModel, TText>> propertySelector,
		in SerializerDirection direction = SerializerDirection.Both,
		in Func<IXmlConverter<IXmlText>>? converter = null
	)
	{
		_propertyMap.Add(new PropertyMap(
			in direction,
			typeof(IXmlText),
			propertySelector.GetProperty(),
			// This isn't used but setting it to null requires a lot more code.
			in TextNamingStrategy.Default,
			converter
		));
	}
}