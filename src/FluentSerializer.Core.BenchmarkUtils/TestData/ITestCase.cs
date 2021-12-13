namespace FluentSerializer.Core.BenchmarkUtils.TestData;

public interface ITestCase
{
	int Count { get; }
	long SizeInBytes { get; }
}