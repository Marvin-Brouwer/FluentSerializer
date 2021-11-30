namespace FluentSerializer.Core.Constants
{
	public readonly struct LineEndings
	{
		public const string LineFeed = "\n";
		public const string CarriageReturn = "\r";

		public const string ReturnLineFeed = CarriageReturn + LineFeed;

		public static readonly string Environment = System.Environment.NewLine;
	}
}
