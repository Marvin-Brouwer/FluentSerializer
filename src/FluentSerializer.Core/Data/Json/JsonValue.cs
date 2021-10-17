﻿using System.Diagnostics;
using System.Text;

namespace FluentSerializer.Core.Data.Json
{
    [DebuggerDisplay(nameof(Value))]
    public sealed class JsonValue : IJsonNode
    {
        public string Name { get; }
        public string? Value { get; }

        public static JsonValue String(string? value = null) => new ($"\"{value}\"");


        public JsonValue(string? value = null)
        {
            const string valueName = "#value";
            Name = valueName;
            Value = value;
        }

        public override string ToString() => ToString(true);
        public string ToString(bool format = true) => WriteTo(new StringBuilder(), format).ToString();
        public StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            // JSON does not support empty property assignment or array members
            if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

           return stringBuilder.Append(Value);
        }
    }
}
