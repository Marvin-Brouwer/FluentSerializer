using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Constants;

namespace FluentSerializer.Core.TestUtils.Helpers
{
	public readonly struct TestStringBuilderPool
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<ITextWriter> StringFastPool = ObjectPoolProvider.CreateStringFastPool(LineEndings.LineFeed);
    }
}
