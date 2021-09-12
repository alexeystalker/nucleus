using System.Collections.Generic;

namespace Nucleus.Core.Models;

public record SiteEntry
{
    public string StringContent { get; init; }
    public byte[] BinaryContent { get; init; }
    public InputFile File { get; init; }
    public Dictionary<string, string> Metadata { get; set; }
}
