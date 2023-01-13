#if NETSTANDARD2_0

using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Core.Dirty.BackwardsCompatibility.NetFramework;

/// <summary>
/// Extensions for backward compatibility
/// </summary>
public static class StringBuilderExtensions
{

	/// <summary>
	/// Append the value of <paramref name="values"/> and join them with a <paramref name="separator"/>
	/// </summary>
	public static StringBuilder AppendJoin<T>(this StringBuilder builder, char separator, IEnumerable<T> values)
	{
		using (IEnumerator<T> en = values.GetEnumerator())
		{
			if (!en.MoveNext())
			{
				return builder;
			}

			T value = en.Current;
			if (value != null)
			{
				builder.Append(value.ToString());
			}

			while (en.MoveNext())
			{
				builder.Append(separator);
				value = en.Current;
				if (value != null)
				{
					builder.Append(value.ToString());
				}
			}
		}
		return builder;
	}

	/// <summary>
	/// Append the value of <paramref name="values"/> and join them with a <paramref name="separator"/>
	/// </summary>
	public static StringBuilder AppendJoin<T>(this StringBuilder builder, string separator, IEnumerable<T> values)
	{
		using (IEnumerator<T> en = values.GetEnumerator())
		{
			if (!en.MoveNext())
			{
				return builder;
			}

			T value = en.Current;
			if (value != null)
			{
				builder.Append(value.ToString());
			}

			while (en.MoveNext())
			{
				builder.Append(separator);
				value = en.Current;
				if (value != null)
				{
					builder.Append(value.ToString());
				}
			}
		}
		return builder;
	}
}

#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Text.StringBuilder))]
#endif