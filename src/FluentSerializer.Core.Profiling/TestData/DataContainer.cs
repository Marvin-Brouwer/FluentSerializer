namespace FluentSerializer.Core.Profiling.TestData
{
    /// <summary>
    /// This class mainly exists to prevent errors about string literals in the generated code.
    /// But, this is also used to give a better representation of what data is actually going in.
    /// </summary>
    public readonly record struct DataContainer<TData>(TData Value, int Size)
    {
        public override string ToString()
        {
            return $"{typeof(TData).Name} [{Size}]";
        }
    }
}
