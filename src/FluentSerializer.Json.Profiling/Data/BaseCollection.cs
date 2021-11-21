using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Profiling.TestData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FluentSerializer.Json.Profiling.Data
{

    public abstract class BaseCollection<TTemplateFile, TData>
        where TData : IDataNode
    {
        private const string ClassDir = "../../../Data";

        protected void GenerateData(int smallSize, int mediumSize, int largeSize)
        {
            var emptyTemplate = File.ReadAllText(Path.Join(ClassDir, $"{typeof(TTemplateFile).Name}.cs"));

            var dataSet = new TestDataSet(largeSize);
            var largeDataSet = dataSet.TestData;
            var mediumDataSet = largeDataSet.Take(mediumSize).ToList();
            var smallDataSet = mediumDataSet.Take(smallSize).ToList();

            GenerateCodeFile(nameof(SmallCollection), smallDataSet, emptyTemplate);
            GenerateCodeFile(nameof(MediumCollection), mediumDataSet, emptyTemplate);
            GenerateCodeFile(nameof(LargeCollection), largeDataSet, emptyTemplate);

            GenerateStringFile(nameof(SmallCollection), smallDataSet);
            GenerateStringFile(nameof(MediumCollection), mediumDataSet);
            GenerateStringFile(nameof(LargeCollection), largeDataSet);
        }

        private void GenerateStringFile(string propertyName, List<ResidentialArea> dataSet)
        {
            var filePath = Path.Join(ClassDir, GetStringFileName(propertyName));
            var objectData = ConvertToData(dataSet).ToList();
            WriteStringContent(objectData, filePath);
        }
        protected void WriteStringContent(List<TData> data, string filePath)
        {
            using var writer = File.CreateText(filePath);
            var stringBuilder = new StringBuilder();

            var dataObject = WrapData(data);
            dataObject.AppendTo(stringBuilder, true, 0, false);
            writer.Write(stringBuilder);

            writer.Flush();
            writer.Close();
        }

        private void GenerateCodeFile(string propertyName, List<ResidentialArea> residentialAreas, string emptyTemplate)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("[System.ComponentModel.Browsable(false)]");
            stringBuilder.AppendLine("[System.Diagnostics.DebuggerHidden]");
            stringBuilder.AppendLine("[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]");
            stringBuilder.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
            stringBuilder.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            
            stringBuilder.AppendLine($"\t\tpublic override System.Collections.Generic.List<{typeof(TData).Name}> {propertyName} => new()");
            stringBuilder.AppendLine("\t\t{");
            GenerateCodeContent(residentialAreas, stringBuilder);
            stringBuilder.Append("\t\t};");

            var fileContents = 
                "using static FluentSerializer.Json.JsonBuilder;" + Environment.NewLine +
                emptyTemplate.Replace("/* PlaceHolder */", stringBuilder.ToString());

            File.WriteAllText(
                Path.Join(ClassDir, $"{typeof(TTemplateFile).Name}.{propertyName}.cs"),
                fileContents);
        }

        private static readonly NotImplementedException _runGeneratorException = new("Please run the generator.");
        protected abstract void GenerateCodeContent(List<ResidentialArea> residentialAreas, StringBuilder stringBuilder);
        protected abstract IEnumerable<TData> ConvertToData(List<ResidentialArea> residentialAreas);

        protected abstract TData WrapData(List<TData> data);
        protected abstract string GetStringFileName(string name);

        public TData GetObjectData(string value)
        {
            return value switch
            {
                "S" => WrapData(SmallCollection),
                "M" => WrapData(MediumCollection),
                "L" => WrapData(LargeCollection),
                _ => WrapData(LargeCollection)
            };
        }
        public IEnumerable<DataContainer<string>> GetStringData()
        {
            yield return new DataContainer<string>(
                File.ReadAllText(Path.Join(ClassDir, GetStringFileName(nameof(SmallCollection)))),
                SmallCollection.Count);
            yield return new DataContainer<string>(
                File.ReadAllText(Path.Join(ClassDir, GetStringFileName(nameof(MediumCollection)))),
                MediumCollection.Count);
            yield return new DataContainer<string>(
                File.ReadAllText(Path.Join(ClassDir, GetStringFileName(nameof(LargeCollection)))),
                LargeCollection.Count);
        }

        public virtual List<TData> SmallCollection => throw _runGeneratorException;
        public virtual List<TData> MediumCollection => throw _runGeneratorException;
        public virtual List<TData> LargeCollection => throw _runGeneratorException;
    }
}
