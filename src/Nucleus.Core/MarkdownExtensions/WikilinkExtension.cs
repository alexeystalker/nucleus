using System.Collections.Generic;
using Markdig;
using Markdig.Renderers;

namespace Nucleus.Core.MarkdownExtensions
{
	public class WikilinkExtension : IMarkdownExtension
	{
		private readonly Dictionary<string, string> _linksDict;

		public WikilinkExtension(Dictionary<string, string> linksDict)
		{
			_linksDict = linksDict;
		}

		public void Setup(MarkdownPipelineBuilder pipeline)
		{
			if(!pipeline.InlineParsers.Contains<WikilinkParser>())
				pipeline.InlineParsers.Insert(0, new WikilinkParser(_linksDict));
		}

		public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) { }
	}
}
