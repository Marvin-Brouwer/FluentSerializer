using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Json.Configuration;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentSerializer.Json.Profiles;

/// <summary>
/// A profile for the JSON serializer to map from
/// </summary>
[ImplicitlyUsed]
public abstract class JsonSerializerProfile : ISerializerProfile<JsonSerializerConfiguration>
{
	private readonly List<IClassMap> _classMaps = new();
	private JsonSerializerConfiguration _configuration = JsonSerializerConfiguration.Default;

	/// <inheritdoc cref="ISerializerProfile{JsonSerializerConfiguration}.Configure" />
	protected abstract void Configure();

	/// <remarks>
	/// Using an explicit interface here so it's not confusing to users of the <see cref="JsonSerializerProfile"/> but it's also not internal.
	/// </remarks>
	[System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough,
	 System.Diagnostics.DebuggerHidden]
	IReadOnlyCollection<IClassMap> ISerializerProfile<JsonSerializerConfiguration>.Configure(in JsonSerializerConfiguration configuration)
	{
		_configuration = configuration;
		Configure();
		return new ReadOnlyCollection<IClassMap>(_classMaps);
	}

	/// <summary>
	/// Configure a mapping for <typeparamref name="TModel"/>
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <param name="direction">Override the <see cref="SerializerDirection"/> of this class mapping (defaults to Both)</param>
	/// <param name="namingStrategy">
	///		Override the <see cref="INamingStrategy"/> for this class mapping. <br />
	///		Defaults to <see cref="JsonSerializerConfiguration.DefaultNamingStrategy"/>
	/// </param>
	/// <returns></returns>
	protected IJsonProfileBuilder<TModel> For<TModel>(
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null)
		where TModel : new()
	{
		var classType = typeof(TModel);
		var propertyMap = new List<IPropertyMap>();
		var builder = new JsonProfileBuilder<TModel>(
			namingStrategy ?? _configuration.DefaultNamingStrategy,
			in propertyMap
		);

		// Store in a tuple for lazy evaluation
		_classMaps.Add(new ClassMap(
			in classType,
			in direction,
			// Not being used, but setting this to null adds much more code
			_configuration.DefaultNamingStrategy,
			propertyMap.AsReadOnly()));

		return builder;
	}
}