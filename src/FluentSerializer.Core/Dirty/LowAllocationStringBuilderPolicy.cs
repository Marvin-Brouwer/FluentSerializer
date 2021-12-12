using Microsoft.Extensions.ObjectPool;
using System.Buffers;
using System.Text;

namespace FluentSerializer.Core.Dirty
{
	public sealed class LowAllocationStringBuilderPolicy : PooledObjectPolicy<ITextWriter>
    {
		private readonly Encoding _encoding;
		private readonly string _newLine;

        public LowAllocationStringBuilderPolicy(Encoding encoding, string newLine)
        {
			_encoding = encoding;
            _newLine = newLine;
        }

        public override ITextWriter Create() => new LowAllocationStringBuilder(_encoding, _newLine, ArrayPool<char>.Shared);

        public override bool Return(ITextWriter obj)
        {
            obj.Clear();
            return true;
        }
    }
}
