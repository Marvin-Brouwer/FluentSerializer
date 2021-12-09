using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FluentSerializer.Core.BenchmarkUtils.Configuration
{
    public sealed class GroupedSlowestToFastestOrderer : IOrderer {
        public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) =>
            benchmarksCase
                .OrderBy(benchmark => benchmark.Parameters["X"])
                .ThenBy(benchmark => benchmark.Descriptor.WorkloadMethodDisplayInfo);

        public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary) =>
            benchmarksCases
                .OrderBy(benchmark => summary[benchmark]?.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo)
                .ThenBy(benchmark => summary[benchmark]?.BenchmarkCase.GetRuntime()?.Name)
                .ThenBy(benchmark => summary[benchmark]?.ResultStatistics?.Mean);

        public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => benchmarkCase.GetRuntime().Name ?? string.Empty;

        public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) =>
            string.Empty + benchmarkCase.Descriptor.WorkloadMethodDisplayInfo + benchmarkCase.GetRuntime().Name;

        public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) =>
            logicalGroups.OrderBy(it => it.Key);

        public bool SeparateLogicalGroups => true;
    }
}