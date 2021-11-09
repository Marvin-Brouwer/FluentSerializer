using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    [DebuggerDisplay(DocumentName)]
    public readonly struct XmlDocument : IXmlDocument
    {
        public IXmlElement? RootElement { get; }
        public IReadOnlyList<IXmlNode> Children => RootElement?.Children ?? new List<IXmlNode>(0);

        private const string DocumentName = "<?xml ?>";
        public string Name => DocumentName;

        public XmlDocument(IXmlElement? root)
        {
            RootElement = root;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            AppendTo(stringBuilder, false);
            return stringBuilder.ToString();
        }

        public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true)
        {
            var stringBuilder = stringBuilders.Get();

            var encoding = writer.Encoding;
            stringBuilder
                // todo skip when parsing
                .Append($"<?xml version=\"1.0\" encoding=\"{encoding.WebName}\"?>")
                .AppendOptionalNewline(format);

            stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
            writer.Write(stringBuilder);

            stringBuilder.Clear();
            stringBuilders.Return(stringBuilder);
        }

        public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            return RootElement?.AppendTo(stringBuilder, format, indent, writeNull) ?? stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj)
        {
            if (obj is not IXmlNode xmlNode) return false;

            return Equals(xmlNode);
        }

        public bool Equals(IXmlNode? obj)
        {
            if (RootElement is null && obj is null) return true;
            if (RootElement is null) return false;
            if (obj is not IXmlDocument otherDocument) return false;

            return RootElement.Equals(otherDocument.RootElement);
        }

        public override int GetHashCode() => HashCode.Combine(Name, RootElement?.GetHashCode() ?? 0);

        #endregion
    }
}
