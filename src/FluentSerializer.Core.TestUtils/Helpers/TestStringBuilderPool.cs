using Microsoft.Extensions.ObjectPool;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Constants;
using System.Text;

namespace FluentSerializer.Core.TestUtils.Helpers
{
	public readonly struct TestStringBuilderPool
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<ITextWriter> StringFastPool = ObjectPoolProvider
			.CreateStringFastPool(Encoding.UTF8, LineEndings.LineFeed);
    }
}
