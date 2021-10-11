using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.Mapping
{
    public sealed class ClassMapScanList : ScanList<Type, IClassMap>
    {
        public ClassMapScanList(IEnumerable<IClassMap> dataTypes) : base(dataTypes) { }

        protected override bool Compare(Type compareTo, IClassMap dataType)
        {
            var concreteCompareTo = Nullable.GetUnderlyingType(compareTo) ?? compareTo;
            return concreteCompareTo.EqualsTopLevel(dataType.ClassType);
        }
    }
}
