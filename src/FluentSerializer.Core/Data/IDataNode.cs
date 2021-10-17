using System.Text;

namespace FluentSerializer.Core.Data
{
    public interface IDataNode
    {
        string Name { get; }

        string ToString(bool format);
        StringBuilder WriteTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true);

    }
}
