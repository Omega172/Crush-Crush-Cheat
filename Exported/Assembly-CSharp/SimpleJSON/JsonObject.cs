using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SimpleJSON
{
	// Token: 0x02000089 RID: 137
	[GeneratedCode("simple-json", "1.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonObject : ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IDictionary<string, object>, IEnumerable
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x0000DD70 File Offset: 0x0000BF70
		public JsonObject()
		{
			this._members = new Dictionary<string, object>(16);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000DD88 File Offset: 0x0000BF88
		public JsonObject(IEqualityComparer<string> comparer)
		{
			this._members = new Dictionary<string, object>(comparer);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000DD9C File Offset: 0x0000BF9C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._members.GetEnumerator();
		}

		// Token: 0x17000022 RID: 34
		public object this[int index]
		{
			get
			{
				return JsonObject.GetAtIndex(this._members, index);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		internal static object GetAtIndex(IDictionary<string, object> obj, int index)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (index >= obj.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num = 0;
			foreach (KeyValuePair<string, object> keyValuePair in obj)
			{
				if (num++ == index)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public void Add(string key, object value)
		{
			this._members.Add(key, value);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000DE6C File Offset: 0x0000C06C
		public bool ContainsKey(string key)
		{
			return this._members.ContainsKey(key);
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000DE7C File Offset: 0x0000C07C
		public ICollection<string> Keys
		{
			get
			{
				return this._members.Keys;
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000DE8C File Offset: 0x0000C08C
		public bool Remove(string key)
		{
			return this._members.Remove(key);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000DE9C File Offset: 0x0000C09C
		public bool TryGetValue(string key, out object value)
		{
			return this._members.TryGetValue(key, out value);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		public ICollection<object> Values
		{
			get
			{
				return this._members.Values;
			}
		}

		// Token: 0x17000025 RID: 37
		public object this[string key]
		{
			get
			{
				return this._members[key];
			}
			set
			{
				this._members[key] = value;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000DEDC File Offset: 0x0000C0DC
		public void Add(KeyValuePair<string, object> item)
		{
			this._members.Add(item.Key, item.Value);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		public void Clear()
		{
			this._members.Clear();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000DF08 File Offset: 0x0000C108
		public bool Contains(KeyValuePair<string, object> item)
		{
			return this._members.ContainsKey(item.Key) && this._members[item.Key] == item.Value;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000DF4C File Offset: 0x0000C14C
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = this.Count;
			foreach (KeyValuePair<string, object> keyValuePair in this._members)
			{
				array[arrayIndex++] = keyValuePair;
				if (--num <= 0)
				{
					break;
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000DFE8 File Offset: 0x0000C1E8
		public int Count
		{
			get
			{
				return this._members.Count;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000DFF8 File Offset: 0x0000C1F8
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		public bool Remove(KeyValuePair<string, object> item)
		{
			return this._members.Remove(item.Key);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000E010 File Offset: 0x0000C210
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this._members.GetEnumerator();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000E024 File Offset: 0x0000C224
		public override string ToString()
		{
			return SimpleJson.SerializeObject(this._members, null);
		}

		// Token: 0x0400029C RID: 668
		private const int DICTIONARY_DEFAULT_SIZE = 16;

		// Token: 0x0400029D RID: 669
		private readonly Dictionary<string, object> _members;
	}
}
