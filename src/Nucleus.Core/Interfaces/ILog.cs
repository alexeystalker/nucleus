namespace Nucleus.Core.Interfaces
{
	public enum Severity
	{
		DEBUG,
		INFO,
		WARN,
		ERROR,
		FATAL,
	}
	public interface ILog
	{
		void Message(Severity severity, string message);
		void Debug(string message);
		void Info(string message);
		void Warn(string message);
		void Error(string message);
		void Fatal(string message);
	}
}
