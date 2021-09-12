using System;
using Nucleus.Core.Extensions;
using Nucleus.Core.Interfaces;

namespace Nucleus.Core.Logging
{
	public class AsyncOutLog : LogBase
	{
		public override void Message(Severity severity, string message) => Console.Out.WriteLineAsync($"{DateTime.Now:O} [{severity.ToFastString()}] {message}");
	}
}
