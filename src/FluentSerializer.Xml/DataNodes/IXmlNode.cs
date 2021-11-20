using FluentSerializer.Core.DataNodes;
using System;

namespace FluentSerializer.Xml.DataNodes
{
    public interface IXmlNode : IDataNode, IEquatable<IXmlNode?> { }
}
