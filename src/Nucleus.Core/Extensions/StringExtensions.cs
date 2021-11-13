namespace Nucleus.Core.Extensions;

public static class StringExtensions
{
	public static bool IsSignificant(this string str) => !string.IsNullOrEmpty(str);
}
