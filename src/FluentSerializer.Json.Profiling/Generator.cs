using FluentSerializer.Json.Profiling.Data;
using System;

namespace FluentSerializer.Json.Profiling
{
    public static class Generator
    {
        [STAThread] public static void Main() =>
            JsonDataCollection.Generate(5000, 15000, 30000);
    }
}
