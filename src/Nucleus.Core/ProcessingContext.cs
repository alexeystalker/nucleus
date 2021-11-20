using System.Collections.Generic;
using Markdig;
using Nucleus.Core.MarkdownExtensions;

namespace Nucleus.Core
{
	public class ProcessingContext
	{
		private ProcessingContext(Dictionary<string, string> linksDir)
		{
			Pipeline = new MarkdownPipelineBuilder()
				.UseEmojiAndSmiley()
				.UseCitations()
				.UseFootnotes()
				.UseMathematics()
				.UsePipeTables()
				.UseYamlFrontMatter()
				.UseWikilinkExtension(linksDir)
				.Build();
		}

		public MarkdownPipeline Pipeline { get; }

		public static ProcessingContext PrepareContext(Dictionary<string, string> linksDir)
		{
			var context = new ProcessingContext(linksDir);

			return context;
		}
	}
}
