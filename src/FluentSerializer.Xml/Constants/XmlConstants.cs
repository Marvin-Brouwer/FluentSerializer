
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Xml.Constants
{
    internal static class XmlConstants
    {
        internal static readonly CustomNamingStrategy TextNodeNamingStrategy = new(DataNodes.Nodes.XmlText.TextName);
    }
}