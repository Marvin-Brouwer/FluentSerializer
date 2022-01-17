using FluentSerializer.Core.Configuration;
using System;
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

		public ITextWriter Clear()
		{
			_stringBuilder.Clear();
			return this;
		}

		public override string ToString()
		{
			return _stringBuilder.ToString();
		}

		#region DirectByteAccess
		private byte[] GetBytes()
		{
			var bytes = new char[_stringBuilder.Length];
			_stringBuilder.CopyTo(0, bytes, _stringBuilder.Length);

			return TextConfiguration.Encoding.GetBytes(bytes);
		}

		public Span<byte> AsSpan() => GetBytes()
			.AsSpan(0, _stringBuilder.Length);

		public Memory<byte> AsMemory() => GetBytes()
			.AsMemory(0, _stringBuilder.Length);
		#endregion
	}
}
