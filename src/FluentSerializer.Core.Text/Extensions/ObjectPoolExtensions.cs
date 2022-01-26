using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Text.Writers;
using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Text.Extensions;

public static class ObjectPoolExtensions
{
	/// <returns>A custom <see cref="ObjectPool{T}"/> of <see cref="ITextWriter"/> implementation that uses the <see cref="System.Text.StringBuilder"/> internally</returns>
	/// <inheritdoc cref="ObjectPoolProviderExtensions.CreateStringBuilderPool(ObjectPoolProvider)"/>
	public static ObjectPool<ITextWriter> CreateStringBuilderPool(this ObjectPoolProvider provider, in ITextConfiguration textConfiguration) =>
		provider.Create(new SystemStringBuilderPolicy(textConfiguration));
}