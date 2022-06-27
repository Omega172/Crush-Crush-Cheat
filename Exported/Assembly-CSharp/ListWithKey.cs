using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000165 RID: 357
public class ListWithKey<TKey, TValue> : IEnumerable, IEnumerable<TValue> where TValue : IComparable<TValue>
{
	// Token: 0x06000A3A RID: 2618 RVA: 0x00053CB8 File Offset: 0x00051EB8
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this._list.GetEnumerator();
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00053CCC File Offset: 0x00051ECC
	public int Count
	{
		get
		{
			return this._list.Count;
		}
	}

	// Token: 0x17000150 RID: 336
	public TValue this[int a]
	{
		get
		{
			return this._list[a];
		}
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00053CEC File Offset: 0x00051EEC
	public bool Contains(TKey key)
	{
		return this._hash.Contains(key);
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00053CFC File Offset: 0x00051EFC
	public IEnumerator<TValue> GetEnumerator()
	{
		return this._list.GetEnumerator();
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00053D10 File Offset: 0x00051F10
	public void Sort()
	{
		this._list.Sort();
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00053D20 File Offset: 0x00051F20
	public void Add(TKey key, TValue value)
	{
		this._hash.Add(key);
		this._list.Add(value);
	}

	// Token: 0x040009D3 RID: 2515
	private HashSet<TKey> _hash = new HashSet<TKey>();

	// Token: 0x040009D4 RID: 2516
	private List<TValue> _list = new List<TValue>();
}
