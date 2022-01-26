using FluentSerializer.Core.Configuration;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentSerializer.Core.Text.Writers
{
	/// <summary>
	/// Internal implementation of <see cref="ITextWriter"/> using <see cref="System.Text.StringBuilder"/><br />
	/// The reason we're using a custom implementation: <br />
	/// - Having a pluggable implementation<br />
	/// - Having control over the newline characters<br />
	/// - Having a clear definition of which methods to use when building any serializer library
	/// </summary>
	internal sealed class SystemStringBuilder : ITextWriter
	{
		public ITextConfiguration TextConfiguration { get; }

		internal readonly StringBuilder StringBuilder;

		public SystemStringBuilder(ITextConfiguration textConfiguration, StringBuilder stringBuilder)
		{
			TextConfiguration = textConfiguration;
			StringBuilder = stringBuilder;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ITextWriter Append(in char value)
		{
			StringBuilder.Append(value);
			return this;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ITextWriter Append(in char value, in int repeat)
		{
			StringBuilder.Append(value, repeat);
			return this;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ITextWriter Append(in string? value)
		{
			if (value is null) return this;

			StringBuilder.Append(value);
			return this;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ITextWriter Append(in ReadOnlySpan<char> value)
		{
			if (value.IsEmpty) return this;

			StringBuilder.Append(value);
			return this;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ITextWriter AppendLineEnding()
		{
			StringBuilder.Append(TextConfiguration.NewLine);
			return this;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ITextWriter Clear()
		{
			StringBuilder.Clear();
			return this;
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public override string ToString()
		{
			return StringBuilder.ToString();
		}

		#region DirectByteAccess
#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		private byte[] GetBytes()
		{
			var bytes = new char[StringBuilder.Length];
			StringBuilder.CopyTo(0, bytes, StringBuilder.Length);

			return TextConfiguration.Encoding.GetBytes(bytes);
		}

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Span<byte> AsSpan() => GetBytes()
			.AsSpan(0, StringBuilder.Length);

#if NET6_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public Memory<byte> AsMemory() => GetBytes()
			.AsMemory(0, StringBuilder.Length);
		#endregion
	}
}
