using Nucleus.Core.Interfaces;

namespace Nucleus.Core.Logging
{
	public abstract class LogBase : ILog
	{
		public abstract void Message(Severity severity, string message);

		public void Debug(string message) => Message(Severity.DEBUG, message);

		public void Info(string message) => Message(Severity.INFO, message);

		public void Warn(string message) => Message(Severity.WARN, message);

		public void Error(string message) => Message(Severity.ERROR, message);

		public void Fatal(string message) => Message(Severity.FATAL, message);
	}
}
