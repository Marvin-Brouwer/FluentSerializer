using BenchmarkDotNet.Loggers;
using System.Text;
using Xunit.Abstractions;

namespace FluentSerializer.Core.Profiling.Move
{
    internal sealed class BenchmarToXUnitLogger : ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly StringBuilder _stringBuilder;
        private bool _skipNewline;

        public BenchmarToXUnitLogger(ITestOutputHelper output)
        {
            _output = output;
            _stringBuilder = new StringBuilder();
        }

        public string Id => nameof(ITestOutputHelper);

        public int Priority => 0;

        public void Flush()
        {
            _output.WriteLine(_stringBuilder.ToString());
            _stringBuilder.Clear();
        }

        public void Write(LogKind logKind, string text)
        {
            _skipNewline = true;
            if (logKind == LogKind.Header) return;
            if (logKind == LogKind.Default) return;
            if (logKind == LogKind.Hint) return;
            
            _skipNewline = false;

            if (logKind == LogKind.Info || logKind == LogKind.Statistic)
            {
                _stringBuilder.Append(text);
                return;
            }

            _stringBuilder.Append($"{logKind}: {text}");
        }

        public void WriteLine()
        {
            if (!_skipNewline)
                _stringBuilder.AppendLine();
            _skipNewline = false;
        }

        public void WriteLine(LogKind logKind, string text)
        {
            _skipNewline = true;
            if (logKind == LogKind.Header) return;
            if (logKind == LogKind.Default) return;
            if (logKind == LogKind.Hint) return;

            _skipNewline = false;

            if (logKind == LogKind.Info || logKind == LogKind.Statistic)
            {
                _stringBuilder.AppendLine(text);
                return;
            }

            _stringBuilder.AppendLine($"{logKind}: {text}");
        }
    }
}
