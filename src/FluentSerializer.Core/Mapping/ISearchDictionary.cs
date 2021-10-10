using System.Collections.Generic;

namespace FluentSerializer.Core.Mapping
{
    public interface ISearchDictionary<TCompareTo, TDataType> : IReadOnlyList<TDataType>
        where TDataType : class
    {
        TDataType? Find(TCompareTo key);
    }
}