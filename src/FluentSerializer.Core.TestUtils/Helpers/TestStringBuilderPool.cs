using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.TestUtils.Extensions;

namespace FluentSerializer.Core.TestUtils.Helpers
{
    public readonly struct TestStringBuilderPool
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringFast> StringFastPool = ObjectPoolProvider.CreateStringFastPool(StringExtensions.LineFeed);
    }
}
