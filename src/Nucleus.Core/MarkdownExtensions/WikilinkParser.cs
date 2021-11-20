using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax.Inlines;

namespace Nucleus.Core.MarkdownExtensions
{
	public class WikilinkParser : InlineParser
	{
		private readonly Dictionary<string, string> _linksDict;

		public WikilinkParser(Dictionary<string, string> linksDict)
		{
			_linksDict = linksDict;
			OpeningCharacters = new[] { '[', '!' };
		}

		public override bool Match(InlineProcessor processor, ref StringSlice slice)
		{
			var currentChar = slice.CurrentChar;

			var isEmbedded = false;
			if(currentChar == '!')
			{
				isEmbedded = true;
				currentChar = slice.NextChar();
			}

			if(currentChar != '[' || (currentChar = slice.NextChar()) != '[')
				return false;

			var link = StringSlice.Empty;
			var start = slice.Start + 1;
			var end = start;
			var notLabel = true;
			while(currentChar != '\0')
			{
				currentChar = slice.NextChar();
				if(notLabel && currentChar == '|')
				{
					link = new StringSlice(slice.Text, start, end);
					notLabel = false;
					start = slice.Start + 1;
					end = start;
					continue;
				}

				if(currentChar == ']' && slice.PeekCharExtra(1) == ']') //This is the end, beautiful friend
				{
					var label = new StringSlice(slice.Text, start, end);

					if(notLabel)
						link = new StringSlice(slice.Text, start, end);

					slice.SkipChar();
					slice.SkipChar();
					var stringLink = link.ToString();
					var linkInline = new LinkInline
					{
						Url = _linksDict.TryGetValue(stringLink, out var dicLink) ? dicLink : stringLink,
						IsImage = isEmbedded
					};
					linkInline.AppendChild(new LiteralInline(label)).IsClosed = true;
					processor.Inline = linkInline;
					return true;
				}

				end = slice.Start;
			}

			return false;
		}
	}
}
