using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Dirty
{
    public sealed class StringFastPooledObjectPolicy : PooledObjectPolicy<ITextWriter>
    {
        private readonly string _newLine;

        public StringFastPooledObjectPolicy(string newLine)
        {
            _newLine = newLine;
        }

        public override ITextWriter Create() => new StringFast(_newLine);

        public override bool Return(ITextWriter obj)
        {
            obj.Clear();
            return true;
        }
    }

    public static class ObjectPoolExtensions
    {
        public static ObjectPool<ITextWriter> CreateStringFastPool(this ObjectPoolProvider provider, string newLine) => 
            provider.Create(new StringFastPooledObjectPolicy(newLine));
    }
}
