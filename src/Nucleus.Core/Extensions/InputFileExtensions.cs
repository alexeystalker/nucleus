using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nucleus.Core.Models;
using Nucleus.Core.Settings;

namespace Nucleus.Core.Extensions
{
	public static class InputFileExtensions
	{
		public static Dictionary<string, InputFile> ToUniqueNameDictionary(this IEnumerable<InputFile> files) => files
			.SelectMany(f => f.UniqueNames().Select(u => (key: u, val: f)))
			.ToDictSafe(t => t.key, t => t.val);

		private static IEnumerable<string> UniqueNames(this InputFile file)
		{
			yield return file.Name;
			if(file.RelDir == ".")
				yield break;
			var relDirTokens = file.RelDir.Split(Path.DirectorySeparatorChar);
			for(var i = 0; i < relDirTokens.Length; i++)
			{
				var lastTokens = relDirTokens.TakeLast(i + 1);
				yield return string.Join("/", lastTokens.Append(file.Name));
			}
		}

		public static string OutputRelUrl(this InputFile file, string subdir)
		{
			var extension = ProcessingConstants.ExtensionsToTransform.TryGetValue(file.Extension, out var newExtension)
				? newExtension
				: file.Extension;

			var relDirTokens = file.RelDir.Split(Path.DirectorySeparatorChar, StringSplitOptions.TrimEntries)
				.Where(dir => !string.Equals(dir, "."));

			var relUrlElements = Enumerable.Empty<string>()
				.Append(subdir)
				.Concat(relDirTokens)
				.Append(file.Name + extension)
				.Where(s => !string.IsNullOrEmpty(s));
			return string.Join("/", relUrlElements);
		}
	}
}
