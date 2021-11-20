using System.Linq;
using Markdig;
using Markdig.Syntax;
using Nucleus.Core.Extensions;
using Nucleus.Core.Interfaces;
using Nucleus.Core.Models;
using Nucleus.Core.Settings;

namespace Nucleus.Core
{
	public class MarkdownProcessor : IEntryProcessor
	{
		private readonly ILog _log;

		public MarkdownProcessor(ILog log)
		{
			_log = log;
		}

		public SiteEntry ProcessEntry(SiteEntry entry, ProcessingContext context)
		{
			if(!entry.StringContent.IsSignificant())
				return entry;
			if(!ProcessingConstants.ExtensionsToTransform.ContainsKey(entry.File.Extension.ToLower()))
				return entry;

			var markdownDocument = Markdown.Parse(entry.StringContent, context.Pipeline);

			if(!entry.StringMetadata.ContainsKey("title"))
			{
				var firstLevelHeader =
					markdownDocument.FirstOrDefault(b => b is HeadingBlock { Level: 1 }) as HeadingBlock;
				var firstLevelHeaderText = firstLevelHeader?.Inline?.FirstOrDefault()?.ToString();
				if(firstLevelHeaderText != null)
					entry.StringMetadata["title"] = firstLevelHeaderText;
			}

			var processedContent = markdownDocument.ToHtml(context.Pipeline);
			return entry with { StringContent = processedContent };
		}
	}
}
