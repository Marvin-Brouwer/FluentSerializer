using FluentSerializer.Core.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Core.Services
{
	public interface ISerializer
	{
		SerializerConfiguration Configuration { get; }

		public string Serialize<TModel>([MaybeNull, AllowNull] TModel? model) where TModel : new();
		[return: MaybeNull] public TModel? Deserialize<TModel>([MaybeNull, AllowNull] string? stringData) where TModel : new();
	}
}
