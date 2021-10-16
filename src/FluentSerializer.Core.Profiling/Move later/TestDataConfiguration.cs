using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Profiling.Move
{
    public abstract class TestDataConfiguration : IEnumerable<object[]>
    {

        public abstract IEnumerable<Type> GetTestClasses();

        IEnumerator<object[]> IEnumerable<object[]>.GetEnumerator() => GetTestClasses().Select(type => new object[] { type }).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<object[]>)this).GetEnumerator();
    }
}
