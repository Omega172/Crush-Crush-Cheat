using System;
using System.Collections.Generic;

namespace Spine.Collections
{
	// Token: 0x020001A9 RID: 425
	public static class CollectionExtensions
	{
		// Token: 0x06000CEE RID: 3310 RVA: 0x0005DF1C File Offset: 0x0005C11C
		public static OrderedDictionary<TKey, TSource> ToOrderedDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToOrderedDictionary(keySelector, null);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0005DF28 File Offset: 0x0005C128
		public static OrderedDictionary<TKey, TSource> ToOrderedDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			OrderedDictionary<TKey, TSource> orderedDictionary = new OrderedDictionary<TKey, TSource>(comparer);
			foreach (TSource tsource in source)
			{
				TKey key = keySelector(tsource);
				orderedDictionary.Add(key, tsource);
			}
			return orderedDictionary;
		}
	}
}
