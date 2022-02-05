using FluentSerializer.Core.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Core.Services;

/// <summary>
/// Generic representation of all FluentSerializers
/// </summary>
public interface ISerializer
{
	/// <inheritdoc cref="SerializerConfiguration"/>
	SerializerConfiguration Configuration { get; }

	/// <summary>
	/// Serialize <paramref name="model"/> to a string representation
	/// </summary>
	public string Serialize<TModel>([MaybeNull, AllowNull] in TModel? model) where TModel : new();
	/// <summary>
	/// Deserialize <paramref name="stringData"/> from a string representation to an instance of <typeparamref name="TModel"/>
	/// </summary>
	[return: MaybeNull] public TModel? Deserialize<TModel>([MaybeNull, AllowNull] in string? stringData) where TModel : new();
}