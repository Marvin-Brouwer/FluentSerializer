using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Mapping
{
    public abstract class TypeDictionary<TCompareTo, TDataType> : ISearchDictionary<TCompareTo, TDataType> 
        where TDataType : class
        where TCompareTo : class
    {
        private IReadOnlyList<TDataType> _storedDataTypes;
        private Dictionary<TCompareTo, TDataType> _cachedMappings = new Dictionary<TCompareTo, TDataType>();

        public TypeDictionary(IEnumerable<TDataType> dataTypes)
        {
            _storedDataTypes = dataTypes.ToList().AsReadOnly();
        }

        public TDataType? Find(TCompareTo type)
        {
            if (_cachedMappings.ContainsKey(type))
                return _cachedMappings[type];

            var matchingType = _storedDataTypes.FirstOrDefault(dataType => Compare(type, dataType));
            _cachedMappings[type] = matchingType;

            return matchingType;
        }

        protected abstract bool Compare(TCompareTo compareTo, TDataType dataType);

        #region IReadonlyList<T>

        public TDataType this[int index] => _storedDataTypes[index];

        public int Count => _storedDataTypes.Count;
        public IEnumerator<TDataType> GetEnumerator() => _storedDataTypes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _storedDataTypes.GetEnumerator();

        #endregion
    }
}
