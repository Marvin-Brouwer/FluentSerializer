using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Core.TestUtils.Helpers;

public readonly struct TestStringBuilderPool
{
	private static readonly ITextConfiguration TextConfiguration = TestStringBuilderConfiguration.Default;
	private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
	public static readonly ObjectPool<ITextWriter> Instance = ObjectPoolProvider.CreateLowAllocationStringBuilderPool(TextConfiguration);
}