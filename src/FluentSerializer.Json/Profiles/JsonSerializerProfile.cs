using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Json.Configuration;

namespace FluentSerializer.Json.Profiles;

[ImplicitlyUsed]
public abstract class JsonSerializerProfile : ISerializerProfile
{
	private readonly List<IClassMap> _classMaps = new();
	private JsonSerializerConfiguration _configuration = JsonSerializerConfiguration.Default;

	protected abstract void Configure();

	/// <remarks>
	/// Using an explicit interface here so it's not confusing to users of the <see cref="JsonSerializerProfile"/> but it's also not internal.
	/// </remarks>
	[System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough, 
	 System.Diagnostics.DebuggerHidden]
	IReadOnlyList<IClassMap> ISerializerProfile.Configure(SerializerConfiguration configuration)
	{
		_configuration = (JsonSerializerConfiguration)configuration;
		Configure();
		return new ReadOnlyCollection<IClassMap>(_classMaps);
	}
        
	protected IJsonProfileBuilder<TModel> For<TModel>(
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null)
		where TModel : new()
	{
		var classType = typeof(TModel);
		var propertyMap = new List<IPropertyMap>();
		var builder = new JsonProfileBuilder<TModel>(
			namingStrategy ?? _configuration.DefaultNamingStrategy,
			propertyMap
		);

		// Store in a tuple for lazy evaluation
		_classMaps.Add(new ClassMap(
			classType, 
			direction,
			// Not being used, but setting this to null adds much more code
			_configuration.DefaultNamingStrategy, 
			propertyMap.AsReadOnly()));

		return builder;
	}
}