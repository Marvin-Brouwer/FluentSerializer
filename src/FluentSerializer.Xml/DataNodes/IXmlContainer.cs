using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Xml.DataNodes
{
    public interface IXmlContainer<out TContainer> : IXmlContainer
        where TContainer : IDataContainer<IXmlNode>
    { }
    public interface IXmlContainer : IDataContainer<IXmlNode>, IXmlNode
    { }
}
