using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes;

/// <summary>
/// An implementation of <see cref="IDataContainer{TValue}"/> for XML
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Warning", "S2326:'TContainer' is not used in the interface",
	Justification = "This type parameter is meant for constraining this interface to the type constraint", Scope = "type", Target = "~T:FluentSerializer.Json.DataNodes.IJsonContainer`1")]
public interface IXmlContainer<out TContainer> : IXmlContainer
	where TContainer : IDataContainer<IXmlNode>
{ }

/// <inheritdoc cref="IXmlContainer{TContainer}"/>
public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode
{ }