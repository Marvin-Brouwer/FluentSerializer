using FluentSerializer.Core.Services;
using FluentSerializer.Xml.Configuration;
using FluentSerializer.Xml.DataNodes;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Xml.Services;

public interface IXmlSerializer : ISerializer
{
	XmlSerializerConfiguration XmlConfiguration { get; }
	[return: MaybeNull] TModel? Deserialize<TModel>([MaybeNull, AllowNull] IXmlElement? element) where TModel: new ();
	[return: MaybeNull] IXmlElement? SerializeToElement<TModel>([MaybeNull, AllowNull] TModel? model);
	[return: MaybeNull] IXmlDocument? SerializeToDocument<TModel>([MaybeNull, AllowNull] TModel? model);
}