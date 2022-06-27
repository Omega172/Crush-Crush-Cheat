using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Spine.Collections
{
	// Token: 0x020001A8 RID: 424
	internal class OrderedDictionaryDebugView<TKey, TValue>
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x0005DEFC File Offset: 0x0005C0FC
		public OrderedDictionaryDebugView(OrderedDictionary<TKey, TValue> dictionary)
		{
			this.dictionary = dictionary;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x0005DF0C File Offset: 0x0005C10C
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<TKey, TValue>[] Items
		{
			get
			{
				return this.dictionary.ToArray<KeyValuePair<TKey, TValue>>();
			}
		}

		// Token: 0x04000B8B RID: 2955
		private readonly OrderedDictionary<TKey, TValue> dictionary;
	}
}
