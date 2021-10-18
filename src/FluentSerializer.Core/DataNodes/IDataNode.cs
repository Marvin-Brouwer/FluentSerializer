using System.Text;

namespace FluentSerializer.Core.DataNodes
{
    public interface IDataNode
    {
        string Name { get; }

        string ToString(bool format);
        StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true);

    }
}
