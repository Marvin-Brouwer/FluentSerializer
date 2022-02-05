using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Xml.Services;

/// <summary>
/// The FluentSerializer for XML
/// </summary>
public interface IXmlSerializer : ISerializer
{
	/// <inheritdoc cref="XmlSerializerConfiguration"/>
	XmlSerializerConfiguration XmlConfiguration { get; }

	/// <summary>
	/// Serialize <paramref name="model"/> to a node representation
	/// </summary>
	[return: MaybeNull] IXmlElement? SerializeToElement<TModel>([MaybeNull, AllowNull] in TModel? model);
	/// <summary>
	/// Serialize <paramref name="model"/> to a document representation
	/// </summary>
	[return: MaybeNull] IXmlDocument? SerializeToDocument<TModel>([MaybeNull, AllowNull] in TModel? model);

	/// <summary>
	/// Deserialize <paramref name="element"/> from a node representation to an instance of <typeparamref name="TModel"/>
	/// </summary>
	[return: MaybeNull] TModel? Deserialize<TModel>([MaybeNull, AllowNull] in IXmlElement? element) where TModel : new();
}