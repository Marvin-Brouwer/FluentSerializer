using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Core.Data
{
    public interface IDataSerialWriter<in TData> where TData : IDataContainer<IDataNode>
    {
        string Write(TData data);
    }
}
