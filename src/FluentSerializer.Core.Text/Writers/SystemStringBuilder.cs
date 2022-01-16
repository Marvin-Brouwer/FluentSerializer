using FluentSerializer.Core.Configuration;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Core.Text.Writers
{
	public sealed class SystemStringBuilder : ITextWriter
	{
		public ITextConfiguration TextConfiguration { get; }

		internal readonly StringBuilder _stringBuilder;

		public SystemStringBuilder(ITextConfiguration textConfiguration, StringBuilder stringBuilder)
		{
			TextConfiguration = textConfiguration;
			_stringBuilder = stringBuilder;
		}

		public ITextWriter Append(in char value)
		{
			_stringBuilder.Append(value);
			return this;
		}

		public ITextWriter Append(in char value, in int repeat)
		{
			_stringBuilder.Append(value, repeat);
			return this;
		}

		public ITextWriter Append(in string? value)
		{
			if (value is null) return this;

			_stringBuilder.Append(value);
			return this;
		}

		public ITextWriter Append(in ReadOnlySpan<char> value)
		{
			if (value.IsEmpty) return this;

			_stringBuilder.Append(value);
			return this;
		}

		public ITextWriter AppendLineEnding()
		{
			_stringBuilder.Append(TextConfiguration.NewLine);
			return this;
		}

		public override string ToString()
		{
			return _stringBuilder.ToString();
		}

		public Span<byte> AsSpan() => TextConfiguration.Encoding
			.GetBytes(_stringBuilder.ToString())
			.AsSpan(0, _stringBuilder.Length);

		public Memory<byte> AsMemory() => TextConfiguration.Encoding
			.GetBytes(_stringBuilder.ToString())
			.AsMemory(0, _stringBuilder.Length);

		public ITextWriter Clear()
		{
			_stringBuilder.Clear();
			return this;
		}
	}
}
