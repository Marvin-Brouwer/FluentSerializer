using System;
using System.Linq.Expressions;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Profiles;

public interface IXmlProfileBuilder<TModel>
	where TModel : new()
{

	public IXmlProfileBuilder<TModel> Attribute<TAttribute>(
		Expression<Func<TModel, TAttribute>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null,
		Func<IXmlConverter<IXmlAttribute>>? converter = null
	);

	public IXmlProfileBuilder<TModel> Child<TAttribute>(
		Expression<Func<TModel, TAttribute>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<INamingStrategy>? namingStrategy = null,
		Func<IXmlConverter<IXmlElement>>? converter = null
	);
        
	public void Text<TText>(
		Expression<Func<TModel, TText>> propertySelector,
		SerializerDirection direction = SerializerDirection.Both,
		Func<IXmlConverter<IXmlText>>? converter = null
	);
}