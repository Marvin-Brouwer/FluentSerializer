using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Converting;
using FluentSerializer.Json.Profiles;
using FluentSerializer.UseCase.Mavenlink.Models;
using FluentSerializer.UseCase.Mavenlink.Models.Entities;
using FluentSerializer.UseCase.Mavenlink.Serializer.Converters;
using FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

using System;
using System.Linq.Expressions;

namespace FluentSerializer.UseCase.Mavenlink.Extensions;

/// <summary>
/// Extension class for ease of profile use
/// </summary>
internal static class ProfileExtensions
{
	private static readonly Func<INamingStrategy> CustomFieldsNamingStrategy = Names.Equal(EntityMappings.GetDataReferenceGroupName(nameof(CustomFieldValue)));

	/// <summary>
	/// Create a property mapping that resolves a reference to a datamodel
	/// </summary>
	public static IJsonProfileBuilder<TEntity> PropertyForReference<TEntity, TProperty>(this IJsonProfileBuilder<TEntity> builder,
		Expression<Func<TEntity, TProperty>> propertySelector)
		where TEntity : IMavenlinkEntity, new()
	{
		return builder.Property(propertySelector,
			namingStrategy: Names.Use.ReferencePointer,
			converter: Converter.For.Reference,
			direction: SerializerDirection.Deserialize
		);
	}

	/// <summary>
	/// Create a property mapping that resolves an array of references to an enumerable of datamodel
	/// </summary>
	public static IJsonProfileBuilder<TEntity> PropertyForReferences<TEntity, TProperty>(this IJsonProfileBuilder<TEntity> builder,
		Expression<Func<TEntity, TProperty>> propertySelector)
		where TEntity : IMavenlinkEntity, new()
	{
		return builder.Property(propertySelector,
			namingStrategy: Names.Use.ReferencesPointer,
			converter: Converter.For.References,
			direction: SerializerDirection.Deserialize
		);
	}

	/// <summary>
	/// Create a property mapping that resolves a custom-field reference to a value
	/// </summary>
	public static IJsonProfileBuilder<TEntity> PropertyForCustomField<TEntity, TProperty>(this IJsonProfileBuilder<TEntity> builder,
		Expression<Func<TEntity, TProperty>> propertySelector,
		Func<INamingStrategy> fieldNamingStrategy)
		where TEntity : IMavenlinkEntity, new()
	{
		return builder.Property(propertySelector,
			namingStrategy: CustomFieldsNamingStrategy,
			converter: Converter.For.CustomFieldReference(fieldNamingStrategy),
			direction: SerializerDirection.Deserialize

		);
	}
}
