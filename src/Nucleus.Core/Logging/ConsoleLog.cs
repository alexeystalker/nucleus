using System;
using Nucleus.Core.Extensions;
using Nucleus.Core.Interfaces;

namespace Nucleus.Core.Logging
{
	public class ConsoleLog : LogBase
	{
		public override void Message(Severity severity, string message) => Console.WriteLine($"{DateTime.Now:O} [{severity.ToFastString()}] {message}");
	}
}
