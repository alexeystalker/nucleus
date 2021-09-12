namespace Nucleus.Core.Models
{
	public record InputFile
	{
		public string Extension { get; init; }
		public string Name { get; init; }
		public string FullName { get; init; }
		public string FullDir { get; init; }
		public string RelDir { get; init; }
	}
}
