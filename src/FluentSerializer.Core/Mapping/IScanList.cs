using System.Collections.Generic;

namespace FluentSerializer.Core.Mapping;

/// <summary>
/// An implementation of <see cref="IReadOnlyList{T}"/> with a predetermined option
/// to scan for a certain <typeparamref name="TScanFor" />
/// </summary>
/// <typeparam name="TScanBy">The type of object required by the scan logic</typeparam>
/// <typeparam name="TScanFor">The collection to store</typeparam>
public interface IScanList<in TScanBy, out TScanFor> : IReadOnlyList<TScanFor>
	where TScanFor : class
{
	TScanFor? Scan(TScanBy key);
}