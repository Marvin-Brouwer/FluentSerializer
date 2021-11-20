using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Dirty
{
    public sealed class StringFastPooledObjectPolicy : PooledObjectPolicy<StringFast>
    {
        internal static readonly StringFastPooledObjectPolicy Default = new();

        public override StringFast Create() => new();

        public override bool Return(StringFast obj)
        {
            obj.Clear();
            return true;
        }
    }

    public static class ObjectPoolExtensions
    {
        public static ObjectPool<StringFast> CreateStringFastPool(this ObjectPoolProvider provider) => provider.Create(StringFastPooledObjectPolicy.Default);
    }
}
