using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using FluentSerializer.Core.BenchmarkUtils.TestData;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FluentSerializer.Core.BenchmarkUtils.Configuration;

public sealed class ValueSizeTestOrderer : IOrderer {
	public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) =>
		benchmarksCase
			.OrderBy(benchmark => ((ITestCase)benchmark.Parameters["Value"]).Count)
			.ThenBy(benchmark => ((ITestCase)benchmark.Parameters["Value"]).SizeInBytes);

	public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary) =>
		benchmarksCases
			.OrderBy(benchmark => summary[benchmark]?.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo)
			.ThenBy(benchmark => ((ITestCase)benchmark.Parameters["Value"]).Count)
			.ThenBy(benchmark => ((ITestCase)benchmark.Parameters["Value"]).SizeInBytes)
			.ThenBy(benchmark => summary[benchmark]?.ResultStatistics?.Mean);

	public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => benchmarkCase.Descriptor.WorkloadMethodDisplayInfo ?? string.Empty;

	public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) =>
		string.Empty + benchmarkCase.Descriptor.WorkloadMethodDisplayInfo;

	public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) =>
		logicalGroups.OrderBy(it => it.Key);

	public bool SeparateLogicalGroups => true;
}