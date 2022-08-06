using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSerializer.Core.Constants;

internal readonly struct NamingConstants
{
	internal const char GenericTypeMarker = '`';

	internal static readonly string ForbiddenNamePattern =
		@"^[" +
			@"\w" + 
			ForbiddenCharacters.Underscore + 
			@"\" + ForbiddenCharacters.Minus + 
			ForbiddenCharacters.Plus +
		@"]*$";

	internal readonly struct ForbiddenCharacters
	{
		internal const char Underscore = '_';
		internal const char Minus = '-';
		internal const char Plus = '+';
	}
}
