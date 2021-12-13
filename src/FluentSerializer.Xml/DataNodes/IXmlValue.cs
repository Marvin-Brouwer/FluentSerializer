using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes;

/// <summary>
/// Representation of a value node in XML, e.g. Text, Comment, CData
/// </summary>
public interface IXmlValue : IDataValue, IXmlNode { }