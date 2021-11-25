using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes
{
    /// <summary>
    /// An implementation of <see cref="IDataContainer{TValue}"/> for XML
    /// </summary>
    public interface IXmlContainer<out TContainer> : IXmlContainer
        where TContainer : IDataContainer<IXmlNode>
    { }

    /// <inheritdoc cref="IXmlContainer{TContainer}"/>
    public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode
    { }
}
