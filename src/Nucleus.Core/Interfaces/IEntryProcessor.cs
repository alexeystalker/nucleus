using Nucleus.Core.Models;

namespace Nucleus.Core.Interfaces;

public interface IEntryProcessor
{
	public SiteEntry ProcessEntry(SiteEntry entry);
}
