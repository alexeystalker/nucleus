using System;
using Nucleus.Core.Interfaces;

namespace Nucleus.Core.Extensions
{
	public static class LogExtensions
	{
		public static string ToFastString(this Severity severity) => severity switch
		{
			Severity.DEBUG => "DEBUG",
			Severity.INFO => "INFO",
			Severity.WARN => "WARN",
			Severity.ERROR => "ERROR",
			Severity.FATAL => "FATAL",
			_ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
		};
	}
}
