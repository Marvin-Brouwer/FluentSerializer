using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Helpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FluentSerializer.Core.BenchmarkUtils.TestData;

public abstract class DataCollectionFactory<TData> where TData : IDataNode
{
	private const int BogusSeed = 98123600;

#if (RELEASE)
	protected virtual int[] ItemCount => new[] { 20, 200, 2000, 20000 };
#else
	protected virtual int[] ItemCount => new[] { 10, 20 };
#endif

	public void GenerateTestCaseFiles()
	{
		var directory = GetDirectory();
		if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

		foreach (var amount in ItemCount) GenerateTestCase(amount, directory);

#pragma warning disable S1215 // "GC.Collect" should not be called
		GC.Collect();
#pragma warning restore S1215 // "GC.Collect" should not be called
		GC.WaitForPendingFinalizers();
	}

	private void GenerateTestCase(int dataCount, string directory)
	{
		var filePath = GetFilePath(directory, dataCount);
		Console.Write($"Generating with bogus with a top level count of {dataCount}; ");
		var (data, houseCount, peopleCount) = BogusConfiguration.Generate(BogusSeed, dataCount);
		Console.ForegroundColor = ConsoleColor.DarkYellow;
		Console.WriteLine($"Data composition: {dataCount:N0}/{houseCount:N0}/{peopleCount:N0}; Total unique items: {dataCount + houseCount + peopleCount:N0}");
		Console.ResetColor();

		try
		{
			Console.Write($"Writing bogus to \"{filePath}\"");

			var objectData = ConvertToData(data, dataCount, houseCount, peopleCount);
			WriteStringContent(objectData, filePath);
		}
		finally
		{
			Console.WriteLine();
		}
	}

#if NETSTANDARD2_0
	private string GetDirectory() => string.Join(Path.PathSeparator.ToString(), Path.GetTempPath(), GetType().Assembly.GetName().Name);
	private string GetFilePath(string directory, int dataCount) => string.Join(Path.PathSeparator.ToString(), directory, GetStringFileName(dataCount));
#else
	private string GetDirectory() => Path.Join(Path.GetTempPath(), GetType().Assembly.GetName().Name);
	private string GetFilePath(string directory, int dataCount) => Path.Join(directory, GetStringFileName(dataCount));
#endif

	private static void WriteStringContent(TData data, string filePath)
	{
		using var fileStream = File.Create(filePath);
		using var bufferedStream = new BufferedStream(fileStream);
		var stringBuilder = TestStringBuilderPool.Default.Get();

		data.AppendTo(ref stringBuilder, true, 0, false);
		Console.Write('.');
#if NETSTANDARD2_0
		var stringSpan = stringBuilder.AsSpan().ToArray();
		bufferedStream.Write(stringSpan, 0, stringSpan.Length);
#else
		bufferedStream.Write(stringBuilder.AsSpan());
#endif

		Console.Write('.');
		bufferedStream.Flush();
		bufferedStream.Close();
		TestStringBuilderPool.Default.Return(stringBuilder);
		Console.Write('.');
	}

	/// <summary>
	/// Convert the generated data set to <see cref="TData"/>
	/// </summary>
	protected abstract TData ConvertToData(List<ResidentialArea> data, int residentialAreaCount, long houseCount, long peopleCount);

	/// <summary>
	/// Generate a filename for this test case, preferably with the right extension.
	/// </summary>
	protected abstract string GetStringFileName(int dataCount);

	/// <summary>
	/// Construct a <see cref="TData"/> object to contain the test case presented in <see cref="GetDataFromSpan(string)"/>
	/// </summary>
	protected abstract TData GetDataFromSpan(string stringValue);

	/// <summary>
	/// A collection of TestCases for writing a data object to string
	/// </summary>
	/// <remarks>
	/// It may seem counter intuitive to first parse serialized data into a data object and
	/// then writing it back to that same serialized format again.
	/// However, it appears that generating the bogus data on test run takes up a lot more memory.
	/// This way we only take that cost on startup and not on every test run.
	/// </remarks>
	public IEnumerable<TestCase<IDataNode>> ObjectTestData
	{
		get
		{
			foreach (var amount in ItemCount)
			{
				using var fileStream = File.OpenRead(GetFilePath(GetDirectory(), amount));
				using var stream = new MemoryStream((int)fileStream.Length);
				fileStream.CopyTo(stream);

				var fileString = Encoding.UTF8.GetString(stream.GetBuffer());
				var data = GetDataFromSpan(fileString);

				yield return new TestCase<IDataNode>(() => data, amount, fileStream.Length);
			}
		}
	}

	/// <summary>
	/// A collection of test cases for parsing string to a data object
	/// </summary>
	public IEnumerable<TestCase<Stream>> StringTestData
	{
		get
		{
			foreach (var amount in ItemCount)
			{
				var testFileInfo = new FileInfo(GetFilePath(GetDirectory(), amount));
				yield return new TestCase<Stream>(
					() => File.OpenRead(testFileInfo.FullName), amount, testFileInfo.Length);
			}
		}
	}
}