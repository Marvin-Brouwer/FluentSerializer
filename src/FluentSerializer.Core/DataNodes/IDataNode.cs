﻿using Microsoft.Extensions.ObjectPool;
using System.IO;
using System.Text;

namespace FluentSerializer.Core.DataNodes
{
    public interface IDataNode
    {
        string Name { get; }

        void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, int indent = 0, bool writeNull = true);
        StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true);

    }
}