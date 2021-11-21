using System;

namespace FluentSerializer.Core.Tests.Extensions
{
    public static class StringExtensions
    {
        public const string LineFeed = "\n";

        /// <summary>
        /// This will convert "\r\n" to <see cref="Environment.NewLine"/>
        /// so tests are compatible with linux.
        /// </summary>
        public static string FixNewLine(this string initialValue) =>
            initialValue
                // Use linux line endings to make everything universal int tests
                .Replace("\r\n", LineFeed);
    }
}
