using FluentSerializer.Core.Extensions;
using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Text;
using System.Text;

namespace FluentSerializer.Core.TestUtils.Helpers;

public readonly struct TestStringBuilderPool
{
	private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();

	public static readonly ObjectPool<ITextWriter> Default =
		ObjectPoolProvider.CreateStringBuilderPool(TestStringBuilderConfiguration.Default);

	public static ITextWriter CreateSingleInstance(StringBuilder? stringBuilder = null) => new SystemStringBuilder(
		TestStringBuilderConfiguration.Default,
		stringBuilder ?? new StringBuilder()
	);
}