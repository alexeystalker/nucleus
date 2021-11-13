using System.Collections.Generic;

namespace Nucleus.Core.Models;

public record SiteEntry
{
    public string StringContent { get; init; }
    public byte[] BinaryContent { get; init; }
    public InputFile File { get; init; }
    public Dictionary<string, string> StringMetadata { get; set; }
    public Dictionary<string, List<string>> ListMetadata { get; set; }
}
