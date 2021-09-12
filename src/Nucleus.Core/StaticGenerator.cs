using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Nucleus.Core.Extensions;
using Nucleus.Core.FileSystem;
using Nucleus.Core.Interfaces;
using Nucleus.Core.Models;

namespace Nucleus.Core
{
	public class StaticGenerator
	{
		private readonly IFileSystemService _fileSystemService;
		private readonly ILog _log;

		public StaticGenerator(IFileSystemService fileSystemService, ILog log)
		{
			_fileSystemService = fileSystemService;
			_log = log;
		}

		public async Task GenerateSite()
		{
			_log.Debug("Generate site.");
			_fileSystemService.PrepareOutputFolder();
			await ProcessPages();
			await ProcessNotes();
			return;
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
			await Task.WhenAll(files.Select(file => ProcessFile(subdir, file)));
		}

		private async Task ProcessFile(string subdir, InputFile inputFile)
		{
			var entry = await _fileSystemService.ReadSiteEntryAsync(inputFile);
			await _fileSystemService.SaveToOutput(subdir, entry);
		}
	}
}
