using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Spine.Collections
{
	// Token: 0x020001A5 RID: 421
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(OrderedDictionaryDebugView<, >))]
	public sealed class OrderedDictionary<TKey, TValue> : IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IList<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x06000CB6 RID: 3254 RVA: 0x0005D7A0 File Offset: 0x0005B9A0
		public OrderedDictionary() : this(0, null)
		{
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0005D7AC File Offset: 0x0005B9AC
		public OrderedDictionary(int capacity) : this(capacity, null)
		{
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0005D7B8 File Offset: 0x0005B9B8
		public OrderedDictionary(IEqualityComparer<TKey> comparer) : this(0, comparer)
		{
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0005D7C4 File Offset: 0x0005B9C4
		public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			this.dictionary = new Dictionary<TKey, int>(capacity, comparer ?? EqualityComparer<TKey>.Default);
			this.keys = new List<TKey>(capacity);
			this.values = new List<TValue>(capacity);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0005D800 File Offset: 0x0005BA00
		int IList<KeyValuePair<!0, !1>>.IndexOf(KeyValuePair<TKey, TValue> item)
		{
			int num;
			if (this.dictionary.TryGetValue(item.Key, out num) && object.Equals(this.values[num], item.Value))
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0005D850 File Offset: 0x0005BA50
		void IList<KeyValuePair<!0, !1>>.Insert(int index, KeyValuePair<TKey, TValue> item)
		{
			this.Insert(index, item.Key, item.Value);
		}

		// Token: 0x17000213 RID: 531
		KeyValuePair<TKey, TValue> IList<KeyValuePair<!0, !1>>.this[int index]
		{
			get
			{
				TKey key = this.keys[index];
				TValue value = this.values[index];
				return new KeyValuePair<TKey, TValue>(key, value);
			}
			set
			{
				TKey tkey = this.keys[index];
				if (this.dictionary.Comparer.Equals(tkey, value.Key))
				{
					this.dictionary[value.Key] = index;
				}
				else
				{
					this.dictionary.Add(value.Key, index);
					this.dictionary.Remove(tkey);
				}
				this.keys[index] = value.Key;
				this.values[index] = value.Value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0005D93C File Offset: 0x0005BB3C
		ICollection<TKey> IDictionary<!0, !1>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0005D944 File Offset: 0x0005BB44
		ICollection<TValue> IDictionary<!0, !1>.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0005D94C File Offset: 0x0005BB4C
		void ICollection<KeyValuePair<!0, !1>>.Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0005D964 File Offset: 0x0005BB64
		bool ICollection<KeyValuePair<!0, !1>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			int index;
			return this.dictionary.TryGetValue(item.Key, out index) && object.Equals(this.values[index], item.Value);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0005D9B4 File Offset: 0x0005BBB4
		void ICollection<KeyValuePair<!0, !1>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "The index is negative or outside the bounds of the collection.");
			}
			int num = 0;
			while (num != this.keys.Count && arrayIndex < array.Length)
			{
				TKey key = this.keys[num];
				TValue value = this.values[num];
				array[arrayIndex] = new KeyValuePair<TKey, TValue>(key, value);
				num++;
				arrayIndex++;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0005DA48 File Offset: 0x0005BC48
		bool ICollection<KeyValuePair<!0, !1>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0005DA4C File Offset: 0x0005BC4C
		bool ICollection<KeyValuePair<!0, !1>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this).Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0005DA78 File Offset: 0x0005BC78
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0005DA80 File Offset: 0x0005BC80
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				return this.dictionary.Comparer;
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0005DA90 File Offset: 0x0005BC90
		public void Add(TKey key, TValue value)
		{
			this.dictionary.Add(key, this.values.Count);
			this.keys.Add(key);
			this.values.Add(value);
			this.version++;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0005DADC File Offset: 0x0005BCDC
		public void Insert(int index, TKey key, TValue value)
		{
			if (index < 0 || index > this.values.Count)
			{
				throw new ArgumentOutOfRangeException("index", index, "The index is negative or outside the bounds of the collection.");
			}
			this.dictionary.Add(key, index);
			for (int num = index; num != this.keys.Count; num++)
			{
				TKey tkey = this.keys[num];
				Dictionary<TKey, int> dictionary2;
				Dictionary<TKey, int> dictionary = dictionary2 = this.dictionary;
				TKey key3;
				TKey key2 = key3 = tkey;
				int num2 = dictionary2[key3];
				dictionary[key2] = num2 + 1;
			}
			this.keys.Insert(index, key);
			this.values.Insert(index, value);
			this.version++;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0005DB94 File Offset: 0x0005BD94
		public bool ContainsKey(TKey key)
		{
			return this.dictionary.ContainsKey(key);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0005DBA4 File Offset: 0x0005BDA4
		public TKey GetKey(int index)
		{
			return this.keys[index];
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0005DBB4 File Offset: 0x0005BDB4
		public int IndexOf(TKey key)
		{
			int result;
			if (this.dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return -1;
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0005DBD8 File Offset: 0x0005BDD8
		public OrderedDictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.KeyCollection(this.dictionary);
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0005DBE8 File Offset: 0x0005BDE8
		public bool Remove(TKey key)
		{
			int index;
			if (this.dictionary.TryGetValue(key, out index))
			{
				this.RemoveAt(index);
				return true;
			}
			return false;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0005DC14 File Offset: 0x0005BE14
		public void RemoveAt(int index)
		{
			TKey key = this.keys[index];
			for (int i = index + 1; i < this.keys.Count; i++)
			{
				TKey tkey = this.keys[i];
				Dictionary<TKey, int> dictionary2;
				Dictionary<TKey, int> dictionary = dictionary2 = this.dictionary;
				TKey key3;
				TKey key2 = key3 = tkey;
				int num = dictionary2[key3];
				dictionary[key2] = num - 1;
			}
			this.dictionary.Remove(key);
			this.keys.RemoveAt(index);
			this.values.RemoveAt(index);
			this.version++;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0005DCAC File Offset: 0x0005BEAC
		public bool TryGetValue(TKey key, out TValue value)
		{
			int index;
			if (this.dictionary.TryGetValue(key, out index))
			{
				value = this.values[index];
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0005DCF0 File Offset: 0x0005BEF0
		public OrderedDictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return new OrderedDictionary<TKey, TValue>.ValueCollection(this.values);
			}
		}

		// Token: 0x1700021A RID: 538
		public TValue this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		// Token: 0x1700021B RID: 539
		public TValue this[TKey key]
		{
			get
			{
				return this.values[this.dictionary[key]];
			}
			set
			{
				int index;
				if (this.dictionary.TryGetValue(key, out index))
				{
					this.keys[index] = key;
					this.values[index] = value;
				}
				else
				{
					this.Add(key, value);
				}
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0005DD84 File Offset: 0x0005BF84
		public void Clear()
		{
			this.dictionary.Clear();
			this.keys.Clear();
			this.values.Clear();
			this.version++;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0005DDC0 File Offset: 0x0005BFC0
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0005DDD0 File Offset: 0x0005BFD0
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			int startVersion = this.version;
			for (int index = 0; index != this.keys.Count; index++)
			{
				TKey key = this.keys[index];
				TValue value = this.values[index];
				yield return new KeyValuePair<TKey, TValue>(key, value);
				if (this.version != startVersion)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
			}
			yield break;
		}

		// Token: 0x04000B82 RID: 2946
		private const string CollectionModifiedMessage = "Collection was modified; enumeration operation may not execute.";

		// Token: 0x04000B83 RID: 2947
		private const string EditReadOnlyListMessage = "An attempt was made to edit a read-only list.";

		// Token: 0x04000B84 RID: 2948
		private const string IndexOutOfRangeMessage = "The index is negative or outside the bounds of the collection.";

		// Token: 0x04000B85 RID: 2949
		private readonly Dictionary<TKey, int> dictionary;

		// Token: 0x04000B86 RID: 2950
		private readonly List<TKey> keys;

		// Token: 0x04000B87 RID: 2951
		private readonly List<TValue> values;

		// Token: 0x04000B88 RID: 2952
		private int version;

		// Token: 0x020001A6 RID: 422
		public sealed class KeyCollection : IEnumerable, ICollection<TKey>, IEnumerable<TKey>
		{
			// Token: 0x06000CD8 RID: 3288 RVA: 0x0005DDEC File Offset: 0x0005BFEC
			internal KeyCollection(Dictionary<TKey, int> dictionary)
			{
				this.dictionary = dictionary;
			}

			// Token: 0x06000CD9 RID: 3289 RVA: 0x0005DDFC File Offset: 0x0005BFFC
			[EditorBrowsable(EditorBrowsableState.Never)]
			bool ICollection<!0>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}

			// Token: 0x06000CDA RID: 3290 RVA: 0x0005DE0C File Offset: 0x0005C00C
			[EditorBrowsable(EditorBrowsableState.Never)]
			void ICollection<!0>.Add(TKey item)
			{
				throw new NotSupportedException("An attempt was made to edit a read-only list.");
			}

			// Token: 0x06000CDB RID: 3291 RVA: 0x0005DE18 File Offset: 0x0005C018
			[EditorBrowsable(EditorBrowsableState.Never)]
			void ICollection<!0>.Clear()
			{
				throw new NotSupportedException("An attempt was made to edit a read-only list.");
			}

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0005DE24 File Offset: 0x0005C024
			[EditorBrowsable(EditorBrowsableState.Never)]
			bool ICollection<!0>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000CDD RID: 3293 RVA: 0x0005DE28 File Offset: 0x0005C028
			[EditorBrowsable(EditorBrowsableState.Never)]
			bool ICollection<!0>.Remove(TKey item)
			{
				throw new NotSupportedException("An attempt was made to edit a read-only list.");
			}

			// Token: 0x06000CDE RID: 3294 RVA: 0x0005DE34 File Offset: 0x0005C034
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000CDF RID: 3295 RVA: 0x0005DE3C File Offset: 0x0005C03C
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				this.dictionary.Keys.CopyTo(array, arrayIndex);
			}

			// Token: 0x1700021E RID: 542
			// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0005DE50 File Offset: 0x0005C050
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x06000CE1 RID: 3297 RVA: 0x0005DE60 File Offset: 0x0005C060
			public IEnumerator<TKey> GetEnumerator()
			{
				return this.dictionary.Keys.GetEnumerator();
			}

			// Token: 0x04000B89 RID: 2953
			private readonly Dictionary<TKey, int> dictionary;
		}

		// Token: 0x020001A7 RID: 423
		public sealed class ValueCollection : IEnumerable, ICollection<TValue>, IEnumerable<TValue>
		{
			// Token: 0x06000CE2 RID: 3298 RVA: 0x0005DE78 File Offset: 0x0005C078
			internal ValueCollection(List<TValue> values)
			{
				this.values = values;
			}

			// Token: 0x06000CE3 RID: 3299 RVA: 0x0005DE88 File Offset: 0x0005C088
			[EditorBrowsable(EditorBrowsableState.Never)]
			bool ICollection<!1>.Contains(TValue item)
			{
				return this.values.Contains(item);
			}

			// Token: 0x06000CE4 RID: 3300 RVA: 0x0005DE98 File Offset: 0x0005C098
			[EditorBrowsable(EditorBrowsableState.Never)]
			void ICollection<!1>.Add(TValue item)
			{
				throw new NotSupportedException("An attempt was made to edit a read-only list.");
			}

			// Token: 0x06000CE5 RID: 3301 RVA: 0x0005DEA4 File Offset: 0x0005C0A4
			[EditorBrowsable(EditorBrowsableState.Never)]
			void ICollection<!1>.Clear()
			{
				throw new NotSupportedException("An attempt was made to edit a read-only list.");
			}

			// Token: 0x1700021F RID: 543
			// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0005DEB0 File Offset: 0x0005C0B0
			[EditorBrowsable(EditorBrowsableState.Never)]
			bool ICollection<!1>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000CE7 RID: 3303 RVA: 0x0005DEB4 File Offset: 0x0005C0B4
			[EditorBrowsable(EditorBrowsableState.Never)]
			bool ICollection<!1>.Remove(TValue item)
			{
				throw new NotSupportedException("An attempt was made to edit a read-only list.");
			}

			// Token: 0x06000CE8 RID: 3304 RVA: 0x0005DEC0 File Offset: 0x0005C0C0
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000CE9 RID: 3305 RVA: 0x0005DEC8 File Offset: 0x0005C0C8
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				this.values.CopyTo(array, arrayIndex);
			}

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0005DED8 File Offset: 0x0005C0D8
			public int Count
			{
				get
				{
					return this.values.Count;
				}
			}

			// Token: 0x06000CEB RID: 3307 RVA: 0x0005DEE8 File Offset: 0x0005C0E8
			public IEnumerator<TValue> GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			// Token: 0x04000B8A RID: 2954
			private readonly List<TValue> values;
		}
	}
}
