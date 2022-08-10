using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FluentSerializer.Core.BenchmarkUtils.Configuration;

public sealed class ClassAndCategoryOrderer : IOrderer {
	public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) =>
		benchmarksCase
			.OrderBy(benchmark => benchmark.Descriptor.Type.FullName)
			.ThenBy(benchmark => benchmark.Descriptor.Categories?.FirstOrDefault())
			.ThenByDescending(benchmark => benchmark.Descriptor.Baseline)
			.ThenBy(benchmark => benchmark.Descriptor.MethodIndex);

	public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary) =>
		benchmarksCases
			.OrderBy(benchmark => summary[benchmark]?.BenchmarkCase.Descriptor.Type.FullName)
			.ThenBy(benchmark => summary[benchmark]?.BenchmarkCase.Descriptor.Categories?.FirstOrDefault())
			.ThenByDescending(benchmark => summary[benchmark]?.BenchmarkCase.Descriptor.Baseline)
			.ThenBy(benchmark => summary[benchmark]?.BenchmarkCase.Descriptor.MethodIndex);

	public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) =>
		benchmarkCase.Descriptor.Type.FullName ?? string.Empty;

	public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) =>
		benchmarkCase.Descriptor.Type.FullName + benchmarkCase.Descriptor.Categories?.FirstOrDefault();

	public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) =>
		logicalGroups.OrderBy(it => it.Key ?? string.Empty);

	public bool SeparateLogicalGroups => true;
}