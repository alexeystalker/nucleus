using System.Collections.Generic;

namespace Nucleus.Core.Settings
{
	public class ProcessingConstants
	{
		public static readonly Dictionary<string, string> ExtensionsToTransform = new Dictionary<string, string>
		{
			[".md"] = ".html"
		};
	}
}
