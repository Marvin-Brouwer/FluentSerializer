using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Data.Xml
{
    public sealed class SerialXmlWriter : IDataSerialWriter<XmlElement>
    {
        private readonly bool _format;
        private readonly bool _writeNull;
        private readonly StringBuilder _stringBuilder;

        public SerialXmlWriter(bool format, bool writeNull)
        {
            _format = format;
            _writeNull = writeNull;

            _stringBuilder = new StringBuilder();
        }

        public string Write(XmlElement data)
        {
            _stringBuilder.Clear();

            WriteElement(data, 0);

            return _stringBuilder.ToString();
        }

        private void WriteElement(XmlElement xmlElement, int indent)
        {
            const char tagStartCharacter = '<';
            const char tagTerminaterCharacter = '/';
            const char tagEndCharacter = '>';
            const char spacer = ' ';

            var children = xmlElement.Children;
            var childIndent = _format ? indent + 1 : 0;

            if (!_writeNull && !children.Any()) return;

            _stringBuilder
                .AppendOptionalNewline(false)
                .Append(tagStartCharacter)
                .Append(xmlElement.Name);

            if (!children.Any())
            {
                _stringBuilder
                    .Append(spacer)
                    .Append(tagTerminaterCharacter)
                    .Append(tagEndCharacter);
                return;
            }

            foreach (var attribute in xmlElement.AttributeNodes)
            {
                _stringBuilder
                    .AppendOptionalNewline(_format)
                    .AppendOptionalIndent(indent, _format)
                    .Append(spacer);

                WriteAttribute(attribute);
            }
            _stringBuilder
                .Append(tagEndCharacter);

            foreach (var child in xmlElement.ElementNodes)
            {
                _stringBuilder
                    .AppendOptionalNewline(_format)
                    .AppendOptionalIndent(childIndent, _format);

                WriteElement(child, childIndent);
            }
            var first = true;
            foreach (var text in xmlElement.TextNodes)
            {
                if (first)
                {
                    first = false;
                    _stringBuilder
                        .AppendOptionalNewline(_format)
                        .AppendOptionalIndent(childIndent, _format);
                }

                WriteText(text);
            }

            _stringBuilder
                .AppendOptionalNewline(_format)
                .AppendOptionalIndent(indent, _format)
                .Append(tagStartCharacter)
                .Append(tagTerminaterCharacter)
                .Append(xmlElement.Name)
                .Append(tagEndCharacter);
        }

        private void WriteAttribute(XmlAttribute xmlAttribute)
        {
            const char wrappingCharacter = '"';
            const char assignmentCharacter = '=';

            if (!_writeNull && xmlAttribute.Value is null) return;

            _stringBuilder
                .Append(xmlAttribute.Name)
                .Append(assignmentCharacter)
                .Append(wrappingCharacter)
                .Append(xmlAttribute.Value)
                .Append(wrappingCharacter);
        }

        private void WriteText(XmlText xmlText)
        {
            if (!_writeNull && xmlText.Value is null) return;

            _stringBuilder.Append(xmlText.Value);
        }
    }
}