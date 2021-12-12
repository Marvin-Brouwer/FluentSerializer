using FluentSerializer.Core.Configuration;
using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Dirty
{
	public static class ObjectPoolExtensions
    {
        public static ObjectPool<ITextWriter> CreateStringFastPool(this ObjectPoolProvider provider, in ITextConfiguration textConfiguration) => 
            provider.Create(new LowAllocationStringBuilderPolicy(textConfiguration));
    }
}
