using FluentSerializer.Core.DataNodes;
using System;
using System.Collections.Generic;
using System.IO;

namespace FluentSerializer.Core.Profiling.TestData
{
    public abstract class DataCollectionFactory<TData> where TData : IDataNode
    {
        private const int BogusSeed = 98123600;
        private TData? _objectTestData;

#if (RELEASE)
        protected virtual int StringItemCount => 50000;
        protected virtual int DataItemCount => 30000;
#else
        protected virtual int StringItemCount => 500;
        protected virtual int DataItemCount => 100;
#endif

        public void GenerateObjectData()
        {
            if (_objectTestData is not null) return;

            Console.WriteLine($"Generating {DataItemCount} bogus items into memory");
            var objectDataSet = BogusConfiguration.Generate(BogusSeed, DataItemCount);
            _objectTestData = ConvertToData(objectDataSet);
        }

        public void GenerateStringFiles()
        {
            GenerateStringFile(nameof(StringTestData), StringItemCount);
            GenerateStringFile(nameof(ObjectTestData), DataItemCount);
        }

        private void GenerateStringFile(string collectionName, int dataCount)
        {
            var directory = GetDirectory();
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var filePath = GetFilePath(directory, collectionName, dataCount);
            Console.WriteLine($"Generating {dataCount} bogus items to \"{filePath}\"");
            var stringDataSet = BogusConfiguration.Generate(BogusSeed, dataCount);

            var objectData = ConvertToData(stringDataSet);
            WriteStringContent(objectData, filePath);
        }

        private string GetDirectory() => Path.Join(Path.GetTempPath(), GetType().Assembly.GetName().Name);
        private string GetFilePath(string directory, string collectionName, int dataCount) => Path.Join(directory, 
            GetStringFileName($"{collectionName}-{dataCount}"));

        protected void WriteStringContent(TData data, string filePath)
        {
            using var writer = File.CreateText(filePath);
            var stringBuilder = new StringFast();

            data.AppendTo(stringBuilder, true, 0, false);
            writer.Write(stringBuilder);

            writer.Flush();
            writer.Close();
        }

        protected abstract TData ConvertToData(List<ResidentialArea> residentialAreas);
        protected abstract string GetStringFileName(string name);

        public TData ObjectTestData
        {
            get
            {
                // This should never happen
                if (_objectTestData is null) GenerateObjectData();
                return _objectTestData!;
            }
        }
        public Stream StringTestData => File.OpenRead(GetFilePath(GetDirectory(), nameof(StringTestData), StringItemCount));
    }
}
