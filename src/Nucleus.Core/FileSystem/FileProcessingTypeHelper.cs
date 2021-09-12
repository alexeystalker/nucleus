using System.Collections.Generic;

namespace Nucleus.Core.FileSystem
{
	public enum FileProcessType
	{
		Copy = 0,
		Markdown
	}
	public static class FileProcessingTypeHelper
	{
		public static FileProcessType GetFileProcessTypeByExtension(string extension) => extension switch
		{
			"md" => FileProcessType.Markdown,
			_ => FileProcessType.Copy
		};
	}
}
