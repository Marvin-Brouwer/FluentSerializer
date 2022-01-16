using FluentSerializer.Core.Configuration;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace FluentSerializer.Core.Text.Writers;

public sealed class SystemStringBuilderPolicy : PooledObjectPolicy<ITextWriter>
{
	private readonly ITextConfiguration _textConfiguration;
	private readonly ObjectPool<StringBuilder> _stringBuilderPool;
	private readonly StringBuilderPooledObjectPolicy _stringBuilderPolicy;

	public SystemStringBuilderPolicy(in ITextConfiguration textConfiguration, in ObjectPoolProvider objectPoolProvider)
	{
		_textConfiguration = textConfiguration;
		_stringBuilderPool = objectPoolProvider.CreateStringBuilderPool();
		_stringBuilderPolicy = new StringBuilderPooledObjectPolicy();
	}

	public override ITextWriter Create() =>
		new SystemStringBuilder(_textConfiguration,
		_stringBuilderPool.Get()
	);

	public override bool Return(ITextWriter obj)
	{
		if (obj is SystemStringBuilder stringBuilder)
		{
			// Reuse system stringbuilder policy to determine whether to clear
			var returned = _stringBuilderPolicy.Return(stringBuilder._stringBuilder);
			if (returned) obj.Clear();
			return returned;
		}

		obj.Clear();
		return true;
	}
}