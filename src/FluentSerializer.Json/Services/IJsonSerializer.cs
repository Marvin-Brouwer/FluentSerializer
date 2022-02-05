using FluentSerializer.Core.Services;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Json.Services;

/// <summary>
/// The FluentSerializer for JSON
/// </summary>
public interface IJsonSerializer : ISerializer
{
	/// <inheritdoc cref="JsonSerializerConfiguration"/>
	JsonSerializerConfiguration JsonConfiguration { get; }

	/// <summary>
	/// Serialize <paramref name="model"/> to a node representation
	/// </summary>
	[return: MaybeNull] IJsonContainer? SerializeToContainer<TModel>(in TModel model);

	/// <summary>
	/// Deserialize <paramref name="element"/> from a node representation to an instance of <typeparamref name="TModel"/>
	/// </summary>
	[return: MaybeNull] TModel? Deserialize<TModel>([MaybeNull, AllowNull] in IJsonContainer? element) where TModel: new ();
}