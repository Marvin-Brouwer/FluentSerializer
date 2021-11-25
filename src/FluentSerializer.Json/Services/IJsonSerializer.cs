using FluentSerializer.Core.Services;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.DataNodes;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Json.Services
{
	public interface IJsonSerializer : ISerializer
	{
		JsonSerializerConfiguration JsonConfiguration { get; }

		[return: MaybeNull] TModel? Deserialize<TModel>([MaybeNull, AllowNull] IJsonContainer? element) where TModel: new ();
		[return: MaybeNull] IJsonContainer? SerializeToContainer<TModel>(TModel model);
	}
}