using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Core.TestUtils.Helpers;

public readonly struct TestStringBuilderPool
{
	private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();

	public static readonly ObjectPool<ITextWriter> Default =
		ObjectPoolProvider.CreateLowAllocationStringBuilderPool(TestStringBuilderConfiguration.Default);
	public static readonly ObjectPool<ITextWriter> NoArrayPool =
		ObjectPoolProvider.CreateLowAllocationStringBuilderPool(TestStringBuilderConfiguration.NoArrayPool);
}