using System;
using System.Linq.Expressions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Converting;

namespace FluentSerializer.Json.Profiles;

/// <summary>
/// Profile builder for configuring JSON properties
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IJsonProfileBuilder<TModel>
	where TModel : new()
{
	/// <summary>
	/// Select a property with the <paramref name="propertySelector"/> to configure how to map it.
	/// </summary>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="propertySelector"></param>
	/// <param name="direction">
	///		Override the <see cref="SerializerDirection"/> of this property mapping (defaults to Both),
	///		only has a use when the class has no direction override
	///	</param>
	/// <param name="namingStrategy">
	///		Override the <see cref="INamingStrategy"/> for this property mapping. <br />
	///		Defaults to <see cref="JsonSerializerConfiguration.DefaultNamingStrategy"/> or
	///		the override in the Class mapping when set
	/// </param>
	/// <param name="converter">
	///		Override the <see cref="IJsonConverter"/> for this property mapping
	///	</param>
	/// <returns></returns>
	public IJsonProfileBuilder<TModel> Property<TProperty>(
		in Expression<Func<TModel, TProperty>> propertySelector,
		in SerializerDirection direction = SerializerDirection.Both,
		in Func<INamingStrategy>? namingStrategy = null,
		in Func<IJsonConverter>? converter = null
	);
}