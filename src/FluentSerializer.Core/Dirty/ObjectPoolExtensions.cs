using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace FluentSerializer.Core.Dirty
{
	public static class ObjectPoolExtensions
    {
        public static ObjectPool<ITextWriter> CreateStringFastPool(
			this ObjectPoolProvider provider,
			Encoding encoding,
			string newLine
		) => 
            provider.Create(new LowAllocationStringBuilderPolicy(encoding, newLine));
    }
}
