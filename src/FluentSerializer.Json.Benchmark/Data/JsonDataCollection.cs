using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.DataNodes;

using System.Collections.Generic;
using System.Linq;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Benchmark.Data;

public sealed class JsonDataCollection : DataCollectionFactory<IJsonObject>
{
	public static readonly JsonDataCollection Default = new();

	protected override string GetStringFileName(int dataCount) => $"{nameof(JsonDataCollection)}-{dataCount}.json";
	protected override IJsonObject ConvertToData(List<ResidentialArea> data, int residentialAreaCount, long houseCount, long peopleCount) =>
		Object(
			Property(nameof(residentialAreaCount), Value(residentialAreaCount.ToString())),
			Property(nameof(houseCount), Value(houseCount.ToString())),
			Property(nameof(peopleCount), Value(peopleCount.ToString())),
			Property(nameof(data), Array(data.Select(area => area.ToJsonElement())))
		);

	protected override IJsonObject GetDataFromSpan(string stringValue) => JsonParser.Parse(stringValue);
}