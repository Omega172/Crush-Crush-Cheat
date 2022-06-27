using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000164 RID: 356
public class FastSortedDictionary<TKey, TValue> : IEnumerable, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
{
	// Token: 0x06000A27 RID: 2599 RVA: 0x00053B34 File Offset: 0x00051D34
	public FastSortedDictionary(IComparer<TKey> comparer)
	{
		this.keys = new Dictionary<TKey, TValue>();
		this.list = new SortedDictionary<TKey, TValue>(comparer);
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00053B54 File Offset: 0x00051D54
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw new NotImplementedException();
	}

	// Token: 0x1700014A RID: 330
	public TValue this[TKey key]
	{
		get
		{
			return this.keys[key];
		}
		set
		{
			if (!this.keys.ContainsKey(key))
			{
				this.list[key] = value;
			}
			this.keys[key] = value;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000A2B RID: 2603 RVA: 0x00053B9C File Offset: 0x00051D9C
	public ICollection<TKey> Keys
	{
		get
		{
			return this.list.Keys;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00053BAC File Offset: 0x00051DAC
	public ICollection<TValue> Values
	{
		get
		{
			return this.list.Values;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00053BBC File Offset: 0x00051DBC
	public int Count
	{
		get
		{
			return this.keys.Count;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00053BCC File Offset: 0x00051DCC
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00053BD0 File Offset: 0x00051DD0
	public void Add(TKey key, TValue value)
	{
		this.keys.Add(key, value);
		this.list.Add(key, value);
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00053BEC File Offset: 0x00051DEC
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		this.Add(item.Key, item.Value);
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00053C04 File Offset: 0x00051E04
	public void Clear()
	{
		this.keys.Clear();
		this.list.Clear();
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00053C1C File Offset: 0x00051E1C
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return this.keys.ContainsKey(item.Key);
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x00053C30 File Offset: 0x00051E30
	public bool ContainsKey(TKey key)
	{
		return this.keys.ContainsKey(key);
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00053C40 File Offset: 0x00051E40
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00053C48 File Offset: 0x00051E48
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return this.list.GetEnumerator();
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x00053C5C File Offset: 0x00051E5C
	public bool Remove(TKey key)
	{
		this.keys.Remove(key);
		return this.list.Remove(key);
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x00053C78 File Offset: 0x00051E78
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return this.Remove(item.Key);
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00053C88 File Offset: 0x00051E88
	public bool TryGetValue(TKey key, out TValue value)
	{
		return this.keys.TryGetValue(key, out value);
	}

	// Token: 0x040009D1 RID: 2513
	private Dictionary<TKey, TValue> keys;

	// Token: 0x040009D2 RID: 2514
	private SortedDictionary<TKey, TValue> list;
}
