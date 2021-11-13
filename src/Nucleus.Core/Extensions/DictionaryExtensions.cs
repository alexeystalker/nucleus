using System.Collections.Generic;

namespace Nucleus.Core.Extensions;

public static class DictionaryExtensions
{
	public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
		this Dictionary<TKey, TValue> reciever,
		Dictionary<TKey, TValue> source)
	{
		foreach(var (key, value) in source)
			reciever[key] = value;

		return reciever;
	}
}
