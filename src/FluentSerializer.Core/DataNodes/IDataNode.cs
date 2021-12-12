using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Constants;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Text;

namespace FluentSerializer.Core.DataNodes
{
    public interface IDataNode : IEquatable<IDataNode?>
    {
        string Name { get; }


		string ToString(SerializerConfiguration configuration)
		{
			var stringBuilder = (ITextWriter)new StringFast(configuration.Encoding, configuration.NewLine);
			stringBuilder = AppendTo(ref stringBuilder);
			return stringBuilder.ToString();
		}


		public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in uint indent = 0)
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

		ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in uint indent = 0, in bool writeNull = true);

    }
}
