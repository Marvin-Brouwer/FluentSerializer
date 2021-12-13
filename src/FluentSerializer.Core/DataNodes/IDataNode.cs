using Microsoft.Extensions.ObjectPool;
using System;
using System.IO;
using System.Text;

namespace FluentSerializer.Core.DataNodes;

public interface IDataNode : IEquatable<IDataNode?>
{
	string Name { get; }

	void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, bool writeNull = true, int indent = 0);
	StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true);

}