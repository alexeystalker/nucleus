using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Nucleus.Core.Extensions;
using Nucleus.Core.Interfaces;
using Nucleus.Core.Models;

namespace Nucleus.Core;

public class StaticGenerator
{
	private readonly IEnumerable<IEntryProcessor> _entryProcessors;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILog _log;

	public StaticGenerator(IFileSystemService fileSystemService, IEnumerable<IEntryProcessor> entryProcessors, ILog log)
	{
		_fileSystemService = fileSystemService;
		_entryProcessors = entryProcessors;
		_log = log;
	}

	public async Task GenerateSite()
	{
		_log.Debug("Generate site.");
		_fileSystemService.PrepareOutputFolder();
		await ProcessPages();
		await ProcessNotes();
	}

	private Task ProcessNotes()
	{
		var notesFiles = _fileSystemService.GetNotesFiles().ToImmutableList();
		return ProcessFiles("notes", notesFiles);
	}

	private Task ProcessPages()
	{
		var pageFiles = _fileSystemService.GetPagesFiles().ToImmutableList();
		return ProcessFiles("", pageFiles);
	}

	private async Task ProcessFiles(string subdir, IReadOnlyCollection<InputFile> files)
	{
		foreach(var inputFile in files)
			_fileSystemService.MakeDirectoryForFile(subdir, inputFile);
		var dict = files.ToUniqueNameDictionary();
		var processed = await Task.WhenAll(files.Select(ProcessFile));
		await Task.WhenAll(processed.Select(entry => _fileSystemService.SaveToOutput(subdir, entry)));
	}

	private async Task<SiteEntry> ProcessFile(InputFile inputFile)
	{
		var entry = await _fileSystemService.ReadSiteEntryAsync(inputFile);
		return _entryProcessors.Aggregate(entry, (current, entryProcessor) => entryProcessor.ProcessEntry(current));
	}
}
