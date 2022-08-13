using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.Profiles;
using FluentSerializer.Xml.Configuration;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentSerializer.Xml.Profiles;

/// <summary>
/// A profile for the JSON serializer to map from
/// </summary>
[ImplicitlyUsed]
public abstract class XmlSerializerProfile : ISerializerProfile<XmlSerializerConfiguration>
{
	private readonly List<IClassMap> _classMaps = new();
	private XmlSerializerConfiguration _configuration = XmlSerializerConfiguration.Default;

	/// <inheritdoc cref="ISerializerProfile{XmlSerializerConfiguration}.Configure" />
	protected abstract void Configure();

	/// <remarks>
	/// Using an explicit interface here so it's not confusing to users of the <see cref="XmlSerializerProfile"/> but it's also not internal.
	/// </remarks>
	[System.Diagnostics.DebuggerNonUserCode, System.Diagnostics.DebuggerStepThrough, 
	 System.Diagnostics.DebuggerHidden]
	IReadOnlyCollection<IClassMap> ISerializerProfile<XmlSerializerConfiguration>.Configure(in XmlSerializerConfiguration configuration)
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
	/// <param name="tagNamingStrategy">
	///		Override the <see cref="INamingStrategy"/> for this class's tag name and element mappings. <br />
	///		Defaults to <see cref="XmlSerializerConfiguration.DefaultClassNamingStrategy"/>
	/// </param>
	/// <param name="attributeNamingStrategy">
	///		Override the <see cref="INamingStrategy"/> for this class's attribute mappings. <br />
	///		Defaults to <see cref="XmlSerializerConfiguration.DefaultPropertyNamingStrategy"/>
	/// </param>
	/// <returns></returns>
	protected IXmlProfileBuilder<TModel> For<TModel>(
		in SerializerDirection direction = SerializerDirection.Both,
		in Func<INamingStrategy>? tagNamingStrategy = null,
		in Func<INamingStrategy>? attributeNamingStrategy = null)
		where TModel : new()
	{
		var classType = typeof(TModel);
		var propertyMap = new List<IPropertyMap>();
		var builder = new XmlProfileBuilder<TModel>(
			attributeNamingStrategy ?? _configuration.DefaultPropertyNamingStrategy,
			in propertyMap
		);

		// Store in a tuple for lazy evaluation
		_classMaps.Add(new ClassMap(
			in classType, 
			in direction,
			tagNamingStrategy ?? _configuration.DefaultClassNamingStrategy, 
			propertyMap.AsReadOnly()));

		return builder;
	}
}