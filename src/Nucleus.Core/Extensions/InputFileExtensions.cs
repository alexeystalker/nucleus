using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nucleus.Core.Models;

namespace Nucleus.Core.Extensions;

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
			yield return Path.Combine(lastTokens.Concat(file.Name.AsSingleItemEnumerable()).ToArray());
		}
	}
}