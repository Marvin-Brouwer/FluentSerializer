using FluentSerializer.Core.Text;

using Microsoft.Extensions.ObjectPool;

using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Simple helper to generalize <see cref="IDataNode"/>s ToString()
/// </summary>
public static class DataNodeExtensions
{
	/// <summary>
	/// Serialize this <see cref="IDataNode"/> to string
	/// </summary>
#if NET6_0_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
	public static string WriteTo(this IDataNode node, in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0)
	{
		var stringBuilder = stringBuilders.Get();
		try
		{
			stringBuilder = node.AppendTo(ref stringBuilder, format, indent, writeNull);
			return stringBuilder.ToString();
		}
		finally
		{
			stringBuilders.Return(stringBuilder);
		}
	}
}
