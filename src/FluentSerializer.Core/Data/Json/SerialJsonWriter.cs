using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Json
{
    public sealed class SerialJsonWriter : IDataSerialWriter<JsonObject>
    {
        private readonly bool _format;
        private readonly bool _writeNull;
        private readonly StringBuilder _stringBuilder;

        public SerialJsonWriter(bool format, bool writeNull)
        {
            _format = format;
            _writeNull = writeNull;

            _stringBuilder = new StringBuilder();
        }

        public string Write(JsonObject data)
        {
            _stringBuilder.Clear();

            WriteObject(data, 0);

            return _stringBuilder.ToString();
        }

        private void WriteNode(IJsonNode node, int indent)
        {
            if (node is JsonObject jsonObject) WriteObject(jsonObject, indent);
            if (node is JsonArray jsonArray) WriteArray(jsonArray, indent);
            if (node is JsonProperty jsonProperty) WriteProperty(jsonProperty, indent);
            if (node is JsonValue jsonValue) WriteValue(jsonValue);
        }

        private void WriteObject(JsonObject jsonObject, int indent)
        {
            const char openingCharacter = '{';
            const char separatorCharacter = ',';
            const char closingCharacter = '}';

            var childIndent = indent + 1;

            _stringBuilder
                .Append(openingCharacter);

            for (var i = 0; i < jsonObject.Children.Count; i++)
            {
                var child = jsonObject.Children[i];

                _stringBuilder
                    .AppendOptionalNewline(_format)
                    .AppendOptionalIndent(childIndent, _format)
                    .AppendNode(child, _format, childIndent);

                if (i != jsonObject.Children.Count - 1) _stringBuilder.Append(separatorCharacter);
            }

            _stringBuilder
                .AppendOptionalNewline(_format)
                .AppendOptionalIndent(indent, _format)
                .Append(closingCharacter);
        }

        private void WriteArray(JsonArray jsonArray, int indent)
        {
            const char openingCharacter = '[';
            const char separatorCharacter = ',';
            const char closingCharacter = ']';

            var childIndent = indent + 1;

            _stringBuilder
                .Append(openingCharacter);

            for (var i = 0; i < jsonArray.Children.Count; i++)
            {
                var child = jsonArray.Children[i];

                _stringBuilder
                    .AppendOptionalNewline(_format)
                    .AppendOptionalIndent(childIndent, _format)
                    .AppendNode(child, _format, childIndent);

                if (i != jsonArray.Children.Count - 1) _stringBuilder.Append(separatorCharacter);
            }

            _stringBuilder
                .AppendOptionalNewline(_format)
                .AppendOptionalIndent(indent, _format)
                .Append(closingCharacter);
        }

        private void WriteProperty(JsonProperty jsonProperty, int indent)
        {
            const char wrappingCharacter = '"';
            const char assignmentCharacter = ':';
            const char spacer = ' ';

            var childValue = jsonProperty.Children.FirstOrDefault();
            if (!_writeNull && childValue is null) return;

            _stringBuilder
                .Append(wrappingCharacter)
                .Append(jsonProperty.Name)
                .Append(wrappingCharacter);

            if (_format) _stringBuilder.Append(spacer);
            _stringBuilder.Append(assignmentCharacter);
            if (_format) _stringBuilder.Append(spacer);

            if (childValue is null) _stringBuilder.Append("null");
            else WriteNode(childValue, indent);
        }

        private void WriteValue(JsonValue jsonValue)
        {
            // JSON does not support empty property assignment or array members
            if (!_writeNull && string.IsNullOrEmpty(jsonValue.Value)) return;

            _stringBuilder.Append(jsonValue.Value);
        }
    }
}
