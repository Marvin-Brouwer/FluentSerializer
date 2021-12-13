using FluentSerializer.Core.Configuration;
using Microsoft.Extensions.ObjectPool;
using System.Buffers;

namespace FluentSerializer.Core.Dirty;

public sealed class LowAllocationStringBuilderPolicy : PooledObjectPolicy<ITextWriter>
{
	private readonly ITextConfiguration _textConfiguration;


	public LowAllocationStringBuilderPolicy(in ITextConfiguration textConfiguration)
	{
		_textConfiguration = textConfiguration;
	}

	public override ITextWriter Create() =>
		new LowAllocationStringBuilder(_textConfiguration, ArrayPool<char>.Shared);

	public override bool Return(ITextWriter obj)
	{
		obj.Clear();
		return true;
	}
}