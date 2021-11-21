using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace FluentSerializer.Core.Tests.Helpers
{
    public readonly struct TestStringBuilderPool
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringBuilder> StringBuilderPool = ObjectPoolProvider.CreateStringBuilderPool();
    }
}
