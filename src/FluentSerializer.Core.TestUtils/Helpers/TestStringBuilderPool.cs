using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Core.TestUtils.Helpers;

public readonly struct TestStringBuilderPool
{
	private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();

	public static readonly ObjectPool<ITextWriter> Default =
		ObjectPoolProvider.CreateStringBuilderPool(TestStringBuilderConfiguration.Default);

	public static ITextWriter CreateSingleInstance() => new SystemStringBuilder(
		TestStringBuilderConfiguration.Default,
		new System.Text.StringBuilder()
	);
}