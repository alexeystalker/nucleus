using System;
using System.Collections.Generic;

namespace Nucleus.Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> AsSingleItemEnumerable<T>(this T obj)
		{
			yield return obj;
		}

		public static Dictionary<TKey, TValue> ToDictSafe<TSource, TKey, TValue>(
			this IEnumerable<TSource> enumerable,
			Func<TSource, TKey> keyFunc,
			Func<TSource, TValue> valueFunc)
		{
			var ret = new Dictionary<TKey, TValue>();
			foreach(var source in enumerable)
			{
				var key = keyFunc(source);
				if(ret.ContainsKey(key))
					continue;
				ret[key] = valueFunc(source);
			}

			return ret;
		}
	}
}
