using FluentSerializer.Core.DataNodes;

using System;

namespace FluentSerializer.Xml.DataNodes;

/// <summary>
/// Generic representation of an XML node
/// </summary>
public interface IXmlNode : IDataNode, IEquatable<IXmlNode?> { }