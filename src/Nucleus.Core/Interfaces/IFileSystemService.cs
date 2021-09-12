using System.Collections.Generic;
using System.Threading.Tasks;
using Nucleus.Core.FileSystem;
using Nucleus.Core.Models;

namespace Nucleus.Core.Interfaces
{
	public interface IFileSystemService
	{
		IEnumerable<InputFile> GetNotesFiles();
		IEnumerable<InputFile> GetPagesFiles();
		IEnumerable<InputFile> GetLayoutFiles();
		void PrepareOutputFolder();
		Task SaveToOutput(string subdir, SiteEntry entry);
		void MakeDirectoryForFile(string subdir, InputFile inputFile);
		Task<SiteEntry> ReadSiteEntryAsync(InputFile inputFile);
	}
}
