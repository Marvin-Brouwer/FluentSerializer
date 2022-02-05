using System;
using FluentSerializer.Core.Configuration;
using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Core.Text;

/// <inheritdoc />
public sealed class SystemStringBuilderPolicy : PooledObjectPolicy<ITextWriter>
{
	private readonly ITextConfiguration _textConfiguration;
	private readonly StringBuilderPooledObjectPolicy _stringBuilderPolicy;

	/// <inheritdoc />
	public SystemStringBuilderPolicy(in ITextConfiguration textConfiguration)
	{
		_textConfiguration = textConfiguration;
		_stringBuilderPolicy = new StringBuilderPooledObjectPolicy
		{
			InitialCapacity = textConfiguration.StringBuilderInitialCapacity,
			MaximumRetainedCapacity = textConfiguration.StringBuilderMaximumRetainedCapacity
		};
	}

	/// <inheritdoc />
	public override ITextWriter Create() =>
		new SystemStringBuilder(_textConfiguration,
		_stringBuilderPolicy.Create()
	);

	/// <inheritdoc />
	public override bool Return(ITextWriter obj)
	{
		if (obj is not SystemStringBuilder stringBuilder)
			throw new NotSupportedException($"The type of {obj.GetType().FullName} is not assignable to {nameof(SystemStringBuilder)}");

		// Reuse system stringbuilder policy to determine whether to clear
		var returned = _stringBuilderPolicy.Return(stringBuilder.StringBuilder);
		if (returned) obj.Clear();
		return returned;
	}
}