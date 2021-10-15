using System.Xml;
using System.Xml.Linq;
using FluentSerializer.Xml.Constants;

namespace FluentSerializer.Xml.Converting.Converters.XNodes
{
    public sealed class XFragment : XElement
    {
        public XFragment() : base(XmlConstants.FragmentNodeDisplayTag) { }
        public XFragment(XObject other) : this() {
            Add(other);
        }
        public XFragment(XFragment other) : base(other) { }

        public XNode CloneNode() => new XFragment(this);
        public override XmlNodeType NodeType => XmlNodeType.DocumentFragment;


    }
}