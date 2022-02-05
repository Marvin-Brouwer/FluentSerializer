using System;
using System.Linq.Expressions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Profiles;

/// <summary>
/// Profile builder for configuring XML properties
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IXmlProfileBuilder<TModel>
	where TModel : new()
{
	/// <summary>
	/// Select a property with the <paramref name="propertySelector"/> to configure how to map it.
	/// </summary>
	/// <typeparam name="TAttribute"></typeparam>
	/// <param name="propertySelector"></param>
	/// <param name="direction">
	///		Override the <see cref="SerializerDirection"/> of this property mapping (defaults to Both),
	///		only has a use when the class has no direction override
	///	</param>
	/// <param name="namingStrategy">
	///		Override the <see cref="INamingStrategy"/> for this property mapping. <br />
	///		Defaults to <see cref="XmlSerializerConfiguration.DefaultPropertyNamingStrategy"/> or
	///		the override in the Class mapping when set
	/// </param>
	/// <param name="converter">
	///		Override the <see cref="IXmlConverter"/> for this property mapping
	///	</param>
	/// <returns></returns>
	public IXmlProfileBuilder<TModel> Attribute<TAttribute>(
		Expression<Func<TModel, TAttribute>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null,
		Func<IXmlConverter<IXmlAttribute>>? converter = null
	);

	/// <summary>
	/// Select a property with the <paramref name="propertySelector"/> to configure how to map it.
	/// </summary>
	/// <typeparam name="TElement"></typeparam>
	/// <param name="propertySelector"></param>
	/// <param name="direction">
	///		Override the <see cref="SerializerDirection"/> of this property mapping (defaults to Both),
	///		only has a use when the class has no direction override
	///	</param>
	/// <param name="namingStrategy">
	///		Override the <see cref="INamingStrategy"/> for this property mapping. <br />
	///		Defaults to <see cref="XmlSerializerConfiguration.DefaultPropertyNamingStrategy"/> or
	///		the override in the Class mapping when set
	/// </param>
	/// <param name="converter">
	///		Override the <see cref="IXmlConverter"/> for this property mapping
	///	</param>
	/// <returns></returns>
	public IXmlProfileBuilder<TModel> Child<TElement>(
		Expression<Func<TModel, TElement>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null,
		Func<IXmlConverter<IXmlElement>>? converter = null
	);

	/// <summary>
	/// Select a property with the <paramref name="propertySelector"/> to configure how to map it.
	/// </summary>
	/// <typeparam name="TText"></typeparam>
	/// <param name="propertySelector"></param>
	/// <param name="direction">
	///		Override the <see cref="SerializerDirection"/> of this property mapping (defaults to Both),
	///		only has a use when the class has no direction override
	///	</param>
	/// <param name="converter">
	///		Override the <see cref="IXmlConverter"/> for this property mapping
	///	</param>
	/// <returns></returns>
	public void Text<TText>(
		Expression<Func<TModel, TText>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<IXmlConverter<IXmlText>>? converter = null
	);
}