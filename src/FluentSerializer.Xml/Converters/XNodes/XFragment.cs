using System.Xml;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters.XNodes
{
    public sealed class XFragment : XElement
    {
        public XFragment() : base("_fragment") { }
        public XFragment(XObject other) : this() {
            Add(other);
        }
        public XFragment(XFragment other) : base(other) { }

        public XNode CloneNode() => new XFragment(this);
        public override XmlNodeType NodeType => XmlNodeType.DocumentFragment;


    }
}