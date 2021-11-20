using System.Collections.Generic;
using Markdig;

namespace Nucleus.Core.MarkdownExtensions
{
	public static class MarkdownPipelineBuilderExtensions
	{
		public static MarkdownPipelineBuilder UseWikilinkExtension(
			this MarkdownPipelineBuilder builder,
			Dictionary<string, string> linksDictionary)
		{
			var extensions = builder.Extensions;

			if(!extensions.Contains<WikilinkExtension>())
				extensions.Add(new WikilinkExtension(linksDictionary));

			return builder;
		}
	}
}
