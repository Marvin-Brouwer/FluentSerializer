using System.Collections.Generic;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;

namespace FluentSerializer.Core.BenchmarkUtils.Configuration;

internal sealed class MethodOnlyDescriptorColumnProvider : IColumnProvider
{
	public static readonly MethodOnlyDescriptorColumnProvider Default = new();

	private MethodOnlyDescriptorColumnProvider() { }

	public IEnumerable<IColumn> GetColumns(Summary summary)
	{
		yield return TargetMethodColumn.Method;
	}
}