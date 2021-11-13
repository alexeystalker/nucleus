using Nucleus.Core.Interfaces;
using Nucleus.Core.Models;

namespace Nucleus.Core;

public class DummyProcessor : IEntryProcessor
{
	public SiteEntry ProcessEntry(SiteEntry entry) => entry;
}
