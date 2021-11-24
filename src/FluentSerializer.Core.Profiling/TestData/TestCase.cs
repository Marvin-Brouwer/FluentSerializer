namespace FluentSerializer.Core.Profiling.TestData
{
    public readonly record struct TestCase<TData>(TData Data, int Count, long SizeInBytes)
    {
        public override string ToString() => $"{GetReadableSize()} {Count,5:(##000)}";

        // Returns the human-readable file size for an arbitrary, 64-bit file size 
        // The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
        private string GetReadableSize()
        {
            // Get absolute value
            long absolute_i = (SizeInBytes < 0 ? -SizeInBytes : SizeInBytes);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (SizeInBytes >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (SizeInBytes >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (SizeInBytes >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (SizeInBytes >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (SizeInBytes >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = SizeInBytes;
            }
            else
            {
                return SizeInBytes.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable /= 1024;
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }
    }
}
