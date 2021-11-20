using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSerializer.Xml.DataNodes.Nodes
{
    /// <inheritdoc cref="IXmlDocument"/>
    [DebuggerDisplay(DocumentName)]
    public readonly struct XmlDocument : IXmlDocument
    {
        private static readonly int TypeHashCode = typeof(XmlDocument).GetHashCode();

        public IXmlElement? RootElement { get; }
        public IReadOnlyList<IXmlNode> Children => RootElement?.Children ?? new List<IXmlNode>(0);

        private const string DocumentName = "<?xml ?>";
        public string Name => DocumentName;

        /// <inheritdoc cref="IXmlDocument"/>
        /// <param name="root">The root element to represent the actual document</param>
        public XmlDocument(IXmlElement? root)
        {
            RootElement = root;
        }

        public override string ToString()
        {
            var stringBuilder = new StringFast();
            AppendTo(stringBuilder, false);
            return stringBuilder.ToString();
        }

        public string WriteTo(ObjectPool<StringFast> stringBuilders, bool format = true, bool writeNull = true, int indent = 0)
        {
            var stringBuilder = stringBuilders.Get();

            try
            {
                // todo fix encoding later
                stringBuilder
                    .Append($"<?xml version=\"1.0\" encoding=\"UTF-16\"?>")
                    .AppendOptionalNewline(format);

                stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
                return stringBuilder.ToString();
            }
            finally
            {
                stringBuilder.Clear();
                stringBuilders.Return(stringBuilder);
            }
        }

        public StringFast AppendTo(StringFast stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
        {
            return RootElement?.AppendTo(stringBuilder, format, indent, writeNull) ?? stringBuilder;
        }

        #region IEquatable

        public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

        public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

        public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

        public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, RootElement);

        #endregion
    }
}
