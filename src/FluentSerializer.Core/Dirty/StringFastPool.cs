using Microsoft.Extensions.ObjectPool;
using System.Buffers;
using System.Text;

namespace FluentSerializer.Core.Dirty
{
    public sealed class StringFastPooledObjectPolicy : PooledObjectPolicy<ITextWriter>
    {
		private readonly Encoding _encoding;
		private readonly string _newLine;

        public StringFastPooledObjectPolicy(Encoding encoding, string newLine)
        {
			_encoding = encoding;
            _newLine = newLine;
        }

        public override ITextWriter Create() => new StringFast(_encoding, _newLine, ArrayPool<char>.Shared);

        public override bool Return(ITextWriter obj)
        {
            obj.Clear();
            return true;
        }
    }

    public static class ObjectPoolExtensions
    {
        public static ObjectPool<ITextWriter> CreateStringFastPool(this ObjectPoolProvider provider, Encoding encoding, string newLine) => 
            provider.Create(new StringFastPooledObjectPolicy(encoding, newLine));
    }
}
