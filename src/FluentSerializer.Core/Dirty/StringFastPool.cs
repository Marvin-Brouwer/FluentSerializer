using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Dirty
{
    public sealed class StringFastPooledObjectPolicy : PooledObjectPolicy<StringFast>
    {
        private readonly string _newLine;

        public StringFastPooledObjectPolicy(string newLine)
        {
            _newLine = newLine;
        }

        public override StringFast Create() => new(_newLine);

        public override bool Return(StringFast obj)
        {
            obj.Clear();
            return true;
        }
    }

    public static class ObjectPoolExtensions
    {
        public static ObjectPool<StringFast> CreateStringFastPool(this ObjectPoolProvider provider, string newLine) => 
            provider.Create(new StringFastPooledObjectPolicy(newLine));
    }
}
