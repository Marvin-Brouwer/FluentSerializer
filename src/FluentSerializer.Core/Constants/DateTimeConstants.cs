using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSerializer.Core.Constants;

/// <summary>
/// Helper class containing constants for date and time formatting
/// </summary>
public readonly struct DateTimeConstants
{
	/// <summary>
	/// ISO 8601 Time representation format
	/// </summary>
	public const string IsoTimeFormat = "'HH':'mm':'ss.FFFFFFFK";
	/// <summary>
	/// ISO 8601 Date representation format
	/// </summary>
	public const string IsoDateFormat = "yyyy'-'MM'-'dd'";
	/// <summary>
	/// ISO 8601 DateTime representation format
	/// </summary>
	public const string IsoDateTimeFormat = $"{IsoDateFormat}T{IsoTimeFormat}";
}
