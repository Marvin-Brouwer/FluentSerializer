using Microsoft.Extensions.ObjectPool;
using System;

namespace FluentSerializer.Core.DataNodes
{
    public interface IDataNode : IEquatable<IDataNode?>
    {
        string Name { get; }

        string WriteTo(ObjectPool<StringFast> stringBuilders, bool format = true, bool writeNull = true, int indent = 0);
        StringFast AppendTo(StringFast stringBuilder, bool format = true, int indent = 0, bool writeNull = true);

    }
}
