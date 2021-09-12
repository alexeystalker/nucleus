using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Nucleus.Core.Interfaces;
using Nucleus.Core.Models;
using Nucleus.Core.Settings;

namespace Nucleus.Core.FileSystem;

public class FileSystemService : IFileSystemService
{
    private readonly ILog _log;
    private readonly NucleusSettings _settings;

    private readonly string[] TextExtensions =
    {
        ".md"
    };

    public FileSystemService(NucleusSettings settings, ILog log)
    {
        _settings = settings;
        _log = log;
    }

    public IEnumerable<InputFile> GetNotesFiles() =>
        GetFilesInternal(_settings.NotesFolderPath, _settings.NotesFolder.FoldersToIgnore);

    public IEnumerable<InputFile> GetPagesFiles() =>
        GetFilesInternal(_settings.PagesFolderPath, _settings.PagesFolder.FoldersToIgnore);

    public IEnumerable<InputFile> GetLayoutFiles() =>
        GetFilesInternal(_settings.LayoutFolderPath, _settings.LayoutFolder.FoldersToIgnore);

    public void PrepareOutputFolder()
    {
        var di = new DirectoryInfo(_settings.OutputDir);
        if(di.Exists)
        {
            _log.Info($"Clearing output folder {_settings.OutputDir}");
            foreach(var directory in di.EnumerateDirectories())
                directory.Delete(true);
            foreach(var file in di.EnumerateFiles())
                file.Delete();
        } else
        {
            _log.Info($"Creating output folder {_settings.OutputDir}");
            di.Create();
        }
    }

    public void MakeDirectoryForFile(string subdir, InputFile inputFile)
    {
        var pathForFileDir = Path.GetFullPath(Path.Combine(_settings.OutputDir, subdir, inputFile.RelDir));
        var di = new DirectoryInfo(pathForFileDir);
        if(!di.Exists)
            di.Create();
    }

    public async Task SaveToOutput(string subdir, SiteEntry entry)
    {
        _log.Debug($"Process file {entry.File.Name}");
        var pathForFile =
            Path.GetFullPath(Path.Combine(_settings.OutputDir, subdir, entry.File.RelDir, entry.File.Name));
        var content = entry.BinaryContent ?? Encoding.UTF8.GetBytes(entry.StringContent);
        var fi = new FileInfo(pathForFile);
        var stream = fi.Create();
        await stream.WriteAsync(content);
        await stream.FlushAsync();
        _log.Debug($"Processing of file {entry.File.FullName} completed.");
    }

    public async Task<SiteEntry> ReadSiteEntryAsync(InputFile inputFile)
    {
        var ret = new SiteEntry
        {
            File = inputFile,
            Metadata = new Dictionary<string, string>()
        };
        if(TextExtensions.Contains(inputFile.Extension))
            ret = ret with { StringContent = await File.ReadAllTextAsync(inputFile.FullName) };
        else
            ret = ret with { BinaryContent = await File.ReadAllBytesAsync(inputFile.FullName) };

        return ret;
    }

    private IEnumerable<InputFile> GetFilesInternal(string folderPath, string[] subfoldersToSkip)
    {
        var di = new DirectoryInfo(folderPath);
        var fileInfos = di.EnumerateFiles("*", SearchOption.AllDirectories);
        return fileInfos
            .Where(fi => ShouldNotSkip(fi.Directory, folderPath, subfoldersToSkip))
            .Select(fi => new InputFile
            {
                Extension = fi.Extension.ToLower(),
                Name = fi.Name,
                FullName = fi.FullName,
                FullDir = fi.DirectoryName,
                RelDir = Path.GetRelativePath(folderPath, fi.DirectoryName ?? "")
            });
    }

    private static bool ShouldNotSkip(DirectoryInfo di, string folderPath, string[] foldersToSkip)
    {
        if(di == null)
            return true;
        if(folderPath.Equals(di.FullName, StringComparison.InvariantCultureIgnoreCase))
            return true;
        return !foldersToSkip.Any(f => f.Equals(Path.GetRelativePath(folderPath, di.FullName),
                StringComparison.InvariantCultureIgnoreCase))
            && ShouldNotSkip(di.Parent, folderPath, foldersToSkip);
    }
}
