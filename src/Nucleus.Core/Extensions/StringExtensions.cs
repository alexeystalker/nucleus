using System;

namespace Nucleus.Core.Extensions
{
	public static class StringExtensions
	{
		public static bool IsSignificant(this string str) => !string.IsNullOrEmpty(str);

		public static string RemoveLast(this string str, string substr)
		{
			var lastIndex = str.LastIndexOf(substr, StringComparison.InvariantCultureIgnoreCase);
			return lastIndex == -1 ? str : str.Remove(lastIndex);
		}
	}
}
