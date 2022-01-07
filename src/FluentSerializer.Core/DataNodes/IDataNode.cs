using Microsoft.Extensions.ObjectPool;
using System;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Core.DataNodes;

public interface IDataNode : IEquatable<IDataNode?>
{
	string Name { get; }

	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0)
	{
		var stringBuilder = stringBuilders.Get();
		try
		{
			stringBuilder = AppendTo(ref stringBuilder, format, indent, writeNull);
			return stringBuilder.ToString();
		}
		finally
		{
			stringBuilders.Return(stringBuilder);
		}
	}

	ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true);

}